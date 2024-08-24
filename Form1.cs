using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AndroidScreencast
{
    public partial class Form1 : Form
    {
        private Thread? _captureThread = null;
        private CancellationTokenSource _cancellationTokenSource;
        private string _adbPath = "adb"; // Ensure adb is in PATH or provide the full path to adb.exe
        private BufferedGraphicsContext _bufferedGraphicsContext;
        private BufferedGraphics _bufferedGraphics;
        private PictureBox pictureBox1;

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true; // Enable double buffering to reduce flickering
            _bufferedGraphicsContext = BufferedGraphicsManager.Current;
            _bufferedGraphicsContext.MaximumBuffer = new Size(this.Width + 1, this.Height + 1);
            _bufferedGraphics = _bufferedGraphicsContext.Allocate(this.CreateGraphics(),
                new Rectangle(0, 0, this.Width, this.Height));
            StartScreencast();
        }

        private void StartScreencast()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _captureThread = new Thread(() => CaptureScreen(_cancellationTokenSource.Token));
            _captureThread.Start();
        }

        private void StopScreencast()
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel(); // Signal the thread to stop
            }
        }

        private void CaptureScreen(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    // Execute adb command to capture the screen
                    var startInfo = new ProcessStartInfo
                    {
                        FileName = _adbPath,
                        Arguments = "exec-out screencap -p",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };

                    using (var process = new Process { StartInfo = startInfo })
                    {
                        process.Start();

                        using (var ms = new MemoryStream())
                        {
                            process.StandardOutput.BaseStream.CopyTo(ms);
                            process.WaitForExit();

                            if (!token.IsCancellationRequested)
                            {
                                ms.Position = 0; // Reset the stream position for reading
                                UpdatePictureBox(ms);
                            }
                        }
                    }

                    // Control the frame rate
                    Thread.Sleep(50); // Reduce sleep time for smoother frame rate
                }
                catch (OperationCanceledException)
                {
                    break; // Exit gracefully if operation is canceled
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error capturing screen: {ex.Message}");
                    StopScreencast();
                    break; // Exit on error
                }
            }
        }

        private void UpdatePictureBox(Stream imageStream)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<Stream>(UpdatePictureBox), imageStream);
                return;
            }

            try
            {
                using (var bmpTemp = new Bitmap(imageStream))
                {
                    pictureBox1.Image?.Dispose(); // Dispose of the previous image to avoid memory leak
                    pictureBox1.Image = new Bitmap(bmpTemp); // Update the PictureBox image
                }

                RenderBufferedGraphics(); // Render the buffered graphics to reduce flicker
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating image: {ex.Message}");
            }
        }

        private void RenderBufferedGraphics()
        {
            Graphics g = pictureBox1.CreateGraphics();
            _bufferedGraphics.Graphics.Clear(this.BackColor);
            _bufferedGraphics.Graphics.DrawImage(pictureBox1.Image, 0, 0, pictureBox1.Width, pictureBox1.Height);
            _bufferedGraphics.Render(g);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopScreencast();
            _captureThread?.Join(); // Wait for the capture thread to finish

            // Forcefully exit the application
            Environment.Exit(0);
        }
    }
}
