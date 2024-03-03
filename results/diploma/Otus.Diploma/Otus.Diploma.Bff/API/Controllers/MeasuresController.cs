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
        private int _num = 1;//000;// 9000;
        private readonly IMeasureService _measureService;
        static private ulong _deviceId = 123;
        static private ulong _success = 0;
        static private ulong _failure = 0;

        public MeasuresController(IMeasureService measureService)
        {
            _measureService = measureService;
        }

        [HttpGet("/measures/start")]
        public async Task<IActionResult> StartMeasures(CancellationToken cancellationToken)
        {
            _deviceId = 123;
            _success = 0;
            _failure = 0;

            return Ok();
        }

        [HttpGet("/measures/result")]
        public async Task<ActionResult> MeasuresResult(CancellationToken cancellationToken)
        {
            return Ok(new { Success  = _success, Failure = _failure });
        }

        [HttpGet("/measures/{device_id}")]
        public async Task<ActionResult<IEnumerable<MeasureDto>?>> GetMeasures([FromRoute(Name = "device_id")] ulong deviceId, CancellationToken cancellationToken)
        {
            try
            {
                IEnumerable<MeasureDto>? res = default;
                var watch = new Stopwatch();
                watch.Start();
                for (var i = 0; i < _num; ++i)
                {
                    if(_deviceId > 2000)
                    {
                        _deviceId = 123;
                    }
                    res = (await _measureService.GetMeasuresAsync(Interlocked.Increment(ref _deviceId), false, cancellationToken)).ToArray();
                    if (i == 0)
                    {
                        //Console.WriteLine($"0 - {watch.ElapsedMilliseconds}");
                    }
                }

                //Console.WriteLine($"total - {watch.ElapsedMilliseconds}");
                ++_success;
                return Ok();
            }
            catch (Exception ex)
            {
                ++_failure;
                return BadRequest();
            }
            //return BadRequest();
        }

        [HttpGet("/measures/replicas/{device_id}")]
        public async Task<ActionResult<IEnumerable<MeasureDto>?>> GetReplicasMeasures([FromRoute(Name = "device_id")] ulong deviceId, CancellationToken cancellationToken)
        {
            try
            {
                IEnumerable<MeasureDto>? res = default;
                var watch = new Stopwatch();
                watch.Start();
                for (var i = 0; i < _num; ++i)
                {
                    if (_deviceId > 2000)
                    {
                        _deviceId = 123;
                    }
                    res = await _measureService.GetMeasuresAsync(Interlocked.Increment(ref _deviceId), true, cancellationToken);
                    if (i == 0)
                    {
                        //Console.WriteLine($"0 - {watch.ElapsedMilliseconds}");
                    }
                }

                //Console.WriteLine($"total - {watch.ElapsedMilliseconds}");
                ++_success;
                return Ok();
            }
            catch (Exception ex)
            {
                ++_failure;
                return BadRequest();
            }
        }
        
        [HttpGet("/queuedmeasures/replicas/{device_id}")]
        public async Task<ActionResult<IEnumerable<MeasureDto>?>> GetReplicasQueuedMeasures([FromRoute(Name = "device_id")] ulong deviceId, CancellationToken cancellationToken)
        {
            //return Ok();
            IEnumerable<MeasureDto>? res = default;
            try
            {
                
                var watch = new Stopwatch();
                watch.Start();
                for (var i = 0; i < _num; ++i)
                {
                    //if (_deviceId > 2000)
                    //{
                    //    _deviceId = 123;
                    //}
                    //res = await _measureService.GetQueuedMeasuresAsync(++_deviceId, true, cancellationToken);
                    res = await _measureService.GetQueuedMeasuresAsync(Interlocked.Increment(ref _deviceId), true, cancellationToken);
                    if (i == 0)
                    {
                        //Console.WriteLine($"0 - {watch.ElapsedMilliseconds}");
                    }
                }

                //Console.WriteLine($"total - {watch.ElapsedMilliseconds}");
                //return Ok(new[] { res.First().Id });// res.ToArray());
                ////, Value2 = (int)e.Value
                //return Ok(res.Select(e => new { e.Id, e.DeviceId, e.Date }).ToArray());
                //return Ok(res.Select(e => new { e.Id }).ToArray());
                ++_success;
                return Ok();
            }
            catch(Exception ex)
            {
                ++_failure;
                return BadRequest();
            }
        }
    }
}
