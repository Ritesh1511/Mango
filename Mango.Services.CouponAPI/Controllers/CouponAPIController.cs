using AutoMapper;
using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Principal;

namespace Mango.Services.CouponAPI.Controllers
{
    [Route("api/[controller]")] ///api/CouponAPI
    [ApiController]
    public class CouponAPIController : ControllerBase
    {
        private readonly AppDbContext  _db;
        private readonly ResponseDTO _response;
        private IMapper _mapper;

        public CouponAPIController(AppDbContext db ,IMapper mapper  )
        {
            _db = db;
            _response = new ResponseDTO();
            _mapper = mapper;

        }


        //////////////////////////////////////////////////////  -- Get all coupons

        [HttpGet]
        public object get()
        {
            try
            {
                IEnumerable<Coupon> ObjList = _db.Coupons.ToList(); //get all coupons from db
                _response.Result = ObjList;
               
            }
             catch(Exception ex)
            {
                _response.IsSuccess = false; //if exception occurs, set IsSuccess to false
                _response.Message = ex.Message;
            }
            return Ok(_response);
        }


        //////////////////////////////////////////////////////  -- Get coupon by id

        [HttpGet]
        [Route("{Id:int}")]
        public object get(int Id)
        {
            try
            {
                Coupon obj = _db.Coupons.First(u => u.CouponId == Id);

                _response.Result =_mapper.Map<CouponDTO>(obj);
               
            }
            catch(Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }


        ////////////////////////////////////////////////////// -- Get coupon by code

        [HttpGet]
        [Route("GetCouponByCode/{code}")] //api/CouponAPI/GetCouponByCode/code
        public object GetCouponByCode(string code)
        {
            try
            {
                Coupon obj = _db.Coupons.First(u => u.CouponCode.ToLower() == code.ToLower()); 

                _response.Result = _mapper.Map<CouponDTO>(obj);

                if(obj == null)
                {
                    _response.Message = "Failed to retrieve by coupon code or coupon might not be present.";
                }

            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }

        ////////////////////////////////////////////////////// --Post(create new object) coupon from body

        [HttpPost]
        public object Post([FromBody] CouponDTO couponDTO)
        {
            try
            {

                Coupon obj = _mapper.Map<Coupon>(couponDTO);
                obj.CouponId = 0; //handling of identity columns in version 9.0 compared to 8.0.

                //In.NET 9, Entity Framework Core may have introduced stricter handling or 
                //    different default behaviors for identity columns. 
                //    Specifically, when adding a new entity, the framework expects 
                //    the identity column to be set to its default value(usually 0 or null) 
                //    to generate a new ID automatically.
                
                    
                    _db.Coupons.Add(obj);


                _db.SaveChanges();

                _response.Result = _mapper.Map<CouponDTO>(obj);

            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }

        ////////////////////////////////////////////////////// --Put(update existing object) coupon from body

        [HttpPut]
        public object Put([FromBody] CouponDTO couponDTO)
        {
            try
            {

                Coupon obj = _mapper.Map<Coupon>(couponDTO);

                _db.Coupons.Update(obj);
                _db.SaveChanges();

                _response.Result = _mapper.Map<CouponDTO>(obj);

            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }

        [HttpDelete]
        public object Delete(int id)
        {
            try
            {

                Coupon obj = _db.Coupons.First(u=>u.CouponId==id);

                _db.Coupons.Remove(obj);
                _db.SaveChanges();

                _response.Result = _mapper.Map<CouponDTO>(obj);
                
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }


    }
}



