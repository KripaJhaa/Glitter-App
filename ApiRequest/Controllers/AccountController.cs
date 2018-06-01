using AppConst;
using Business;
using ModelsDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ApiRequest.Controllers
{
    public class AccountController : ApiController
    {
        UserBusinessLogic userBusinessLogic = new UserBusinessLogic();
        
        
        [AllowCrossSiteJson]
        [Route("api/Account/LogOut")]
        [HttpGet]
        public IHttpActionResult LoggingOut()
        {
            //var session = HttpContext.Current.Session;

            HttpContext.Current.Session["EmailId"] = 0;
           return Ok(HttpContext.Current.Session["EmailId"]);
                
        }



        [AllowCrossSiteJson]
        [Route("api/Account/Login")]
        [HttpPost]
        public IHttpActionResult Login([FromBody] User user)
        {
            bool IsUserExist = userBusinessLogic.IsValidUser(user);
                
            if (IsUserExist == true)    
            {
                 
                HttpContext.Current.Session["EmailId"] = user.EmailId;

                /*
                if(session["EmailId"].Equals (user.EmailId))
                {
                    Console.WriteLine("Yes Session Exist" + session["EmailId"]);
                }
                */
                //SessionClass.sessionId = session["EmailId"];
                return Ok(HttpContext.Current.Session["EmailId"]);
            }
            else
                return NotFound();
        }


        [AllowCrossSiteJson]
        [Route("api/Account/Register")]
        [HttpPost]
        public IHttpActionResult Register([FromBody] User user)
        {
            bool IsUserExist = userBusinessLogic.IsUserPresentAlready(user);

            if (IsUserExist == true)
            {
                return NotFound();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    userBusinessLogic.SaveUserData(user);
                }

                return Ok();
            }

        }

        


        [AllowCrossSiteJson]
        [Route("api/Account/addTweet")]
        [HttpPost]
        public IHttpActionResult AddTweet([FromBody] Tweet tweetObj)
        {
            bool result = userBusinessLogic.AddTweet(tweetObj);

            if (result == false)
            {
                return NotFound(); 
            }
            else
            {
               return  Ok();
            }
        }

        [AllowCrossSiteJson]
        [Route("api/Account/editTweet")]
        [HttpPost]
        public IHttpActionResult EditTweet([FromBody] Tweet tweetObj)
        {
            bool result = userBusinessLogic.EditTweet(tweetObj);

            if (result == false)
            {
                return NotFound();
            }
            else
            {
                return Ok();
            }
        }

        [AllowCrossSiteJson]
        [Route("api/Account/getAllTweet")]
        [HttpPost]
        public IHttpActionResult GetAllTweet([FromBody] User userObj)
        {
            IList<Tweet> ListOfTweet = userBusinessLogic.GetAllTweets(userObj);
            if (ListOfTweet == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(ListOfTweet);
            }
        }

        [AllowCrossSiteJson]
        [Route("api/Account/followUser")]
        [HttpPost]
        public IHttpActionResult FollowUser([FromBody] UserLink userLinkObj)
        {
            if(userLinkObj.FolloweeId!= null && userLinkObj.FollowerId != null)
            {
                userBusinessLogic.Follow(userLinkObj.FolloweeId,userLinkObj.FollowerId);
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }


        [AllowCrossSiteJson]
        [Route("api/Account/getFollower")]
        [HttpPost]
        public IHttpActionResult GetFollower([FromBody] User userObj)
        {
           IList<string> UserList=  userBusinessLogic.GetAllFollow(userObj);

            if (UserList!=null )
            {
                
                return Ok(UserList);
            }
            else
            {
                return NotFound();
            }
        }


        [AllowCrossSiteJson]
        [Route("api/Account/getFollowee")]
        [HttpPost]
        public IHttpActionResult GetFollowee([FromBody] User userObj)
        {
            IList<string> UserList = userBusinessLogic.GetAllFollowee(userObj);

            if (UserList != null)
            {

                return Ok(UserList);
            }
            else
            {
                return NotFound();
            }
        }


        [AllowCrossSiteJson]
        [Route("api/Account/searchPostTag")]
        [HttpPost]
        public IHttpActionResult SearchPostTag([FromBody] Hashtag hastagObj)
        {
            IList<User> UserList = userBusinessLogic.GetAllTweetBasedOnHashTags(hastagObj);

            if (UserList != null)
            {

                return Ok(UserList);
            }
            else
            {
                return NotFound();
            }
        }


        [AllowCrossSiteJson]
        [Route("api/Account/searchPostPeople")]
        [HttpPost]
        public IHttpActionResult SearchPostPeople([FromBody] User userObj)
        {
            IList<User> UserList = userBusinessLogic.GetAllTweetBasedOnPeople(userObj);

            if (UserList != null)
            {

                return Ok(UserList);
            }
            else
            {
                return NotFound();
            }
        }





        [AllowCrossSiteJson]
        [Route("api/Account/likeTweet")]
        [HttpPost]
        public IHttpActionResult LikeTweet([FromBody] Tweet tweeObj)
        {
            bool result = userBusinessLogic.LikeTweet(tweeObj);

            if (result)
            {

                return Ok();
            }
            else
            {
                return NotFound();
            }
        }


        [AllowCrossSiteJson]
        [Route("api/Account/dislikeTweet")]
        [HttpPost]
        public IHttpActionResult DisLikeTweet([FromBody] Tweet tweeObj)
        {
            bool result = userBusinessLogic.DisLikeTweet(tweeObj);

            if (result)
            {

                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

    }
}