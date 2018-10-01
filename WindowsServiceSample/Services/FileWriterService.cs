using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsServiceSample.Services
{
    public class FileWriterService : IHostedService, IDisposable
    {
        private readonly ITestService _testService;
        private const string Path = @"d:\TestApplication.txt";

        private Timer _timer;

        public FileWriterService(ITestService testService)
        {
            _testService = testService;
        }


        public Task StartAsync(CancellationToken cancellationToken)
        {
            _testService.TestMethod();

            WriteTimeToFile("Start Application");
            _timer = new Timer(
                (e) => WriteTimeToFile(),
                null,
                TimeSpan.Zero,
                TimeSpan.FromMinutes(1));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            WriteTimeToFile("Stop Application");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void WriteTimeToFile(string text = null)
        {
            text = text ?? DateTime.UtcNow.ToString("O");

            if (!File.Exists(Path))
            {
                using (var sw = File.CreateText(Path))
                {
                    sw.WriteLine(text);
                }
            }
            else
            {
                using (var sw = File.AppendText(Path))
                {
                    sw.WriteLine(text);
                }
            }
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}