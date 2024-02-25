using AutoMapper;
using Bff.API.Dtos;
using Bff.Infrastructure.gRpc.Services.Interfaces;
using Common.API.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Bff.API.Controllers
{
    [ApiController]
    public class MeasuresController : ControllerBase
    {
        private int _num = 1;// 9000;
        private readonly IMeasureService _measureService;

        public MeasuresController(IMeasureService measureService)
        {
            _measureService = measureService;
        }

        [HttpGet("/measures/{device_id}")]
        public async Task<ActionResult<IEnumerable<MeasureDto>?>> GetMeasures([FromRoute(Name = "device_id")] ulong deviceId, CancellationToken cancellationToken)
        {
            IEnumerable<MeasureDto>? res = default;
            var watch = new Stopwatch();
            watch.Start();
            for (var i = 0; i < _num; ++i)
            {
                res = await _measureService.GetMeasuresAsync(deviceId++, false, cancellationToken);
                if (i == 0)
                {
                    Console.WriteLine($"0 - {watch.ElapsedMilliseconds}");
                }
            }

            Console.WriteLine($"total - {watch.ElapsedMilliseconds}");
            return Ok(res);
        }

        [HttpGet("/measures/replicas/{device_id}")]
        public async Task<ActionResult<IEnumerable<MeasureDto>?>> GetReplicasMeasures([FromRoute(Name = "device_id")] ulong deviceId, CancellationToken cancellationToken)
        {
            IEnumerable<MeasureDto>? res = default;
            var watch = new Stopwatch();
            watch.Start();
            for (var i = 0; i < _num; ++i)
            {
                res = await _measureService.GetMeasuresAsync(deviceId++, true, cancellationToken);
                if (i == 0)
                {
                    Console.WriteLine($"0 - {watch.ElapsedMilliseconds}");
                }
            }

            Console.WriteLine($"total - {watch.ElapsedMilliseconds}");
            return Ok(res);
        }
        static private ulong _deviceId = 123;
        [HttpGet("/queuedmeasures/replicas/{device_id}")]
        public async Task<ActionResult<IEnumerable<MeasureDto>?>> GetReplicasQueuedMeasures([FromRoute(Name = "device_id")] ulong deviceId, CancellationToken cancellationToken)
        {
            IEnumerable<MeasureDto>? res = default;
            var watch = new Stopwatch();
            watch.Start();
            for (var i = 0; i < _num; ++i)
            {
                res = await _measureService.GetQueuedMeasuresAsync(++_deviceId, true, cancellationToken);
                if (i == 0)
                {
                    Console.WriteLine($"0 - {watch.ElapsedMilliseconds}");
                }
            }

            Console.WriteLine($"total - {watch.ElapsedMilliseconds}");
            return Ok(res);
        }
    }
}
