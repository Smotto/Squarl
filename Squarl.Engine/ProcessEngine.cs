using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using Bitmap = Avalonia.Media.Imaging.Bitmap;

namespace Squarl.Engine;

public class ProcessEngine
{
    /// <summary>
    /// Grab every single process running on the client's machine.
    /// </summary>
    /// <returns>
    /// A Task needed to run to generate a list of processes.
    /// </returns>
    public static async Task<IEnumerable<Process>?> GrabAllRunningProcesses()
    {
        return await Task.Run(Process.GetProcesses);
    }

    public static async Task<IEnumerable<Process>?> GrabAllRunningApplications()
    {
        Process[] allProcesses = await Task.Run(Process.GetProcesses);

        List<Process> Function()
        {
            return allProcesses!
                .Where(process => process.MainWindowHandle != IntPtr.Zero 
                                  && process.MainWindowHandle != Process.GetCurrentProcess().MainWindowHandle).ToList();
        }

        return await Task.Run(Function);
    }

    public static Task<Icon?> GrabProcessIcon(Process process)
    {
        Task<Icon?> ico = Task.Run(() => Icon.ExtractAssociatedIcon(process.MainModule.FileName));
        return ico;
    }
}

public static class ImageExtensions
{
    public static Task<Bitmap?> ConvertToAvaloniaBitmap(this Image? bitmap)
    {
        return Task.Run(() =>
        {
            if (bitmap == null)
                return null;
            System.Drawing.Bitmap bitmapTmp = new System.Drawing.Bitmap(bitmap);
            var bitmapdata = bitmapTmp.LockBits(
                rect: new Rectangle(0, 0, bitmapTmp.Width, bitmapTmp.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format32bppArgb);
            Bitmap bitmap1 = new Bitmap(Avalonia.Platform.PixelFormat.Bgra8888, Avalonia.Platform.AlphaFormat.Premul,
                bitmapdata.Scan0,
                new Avalonia.PixelSize(bitmapdata.Width, bitmapdata.Height),
                new Avalonia.Vector(96, 96),
                bitmapdata.Stride);
            bitmapTmp.UnlockBits(bitmapdata);
            bitmapTmp.Dispose();
            return bitmap1;
        });
    }
}
