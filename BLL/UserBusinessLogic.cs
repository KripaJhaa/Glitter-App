using DataAccess;
using ModelsDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class UserBusinessLogic
    {
        UserDAC userDAC = new UserDAC();
        public bool IsValidUser(User user)
        {
            bool UserExistAlready = userDAC.IsValidUser(user);
            if(UserExistAlready == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsUserPresentAlready(User user)
        {
            bool IsUserPresent = userDAC.IsUserExist(user);

            if(IsUserPresent == true)
            {
                return true;
            }

            else
            {
                return false;
            }

        }

        public User SaveUserData(User user)
        {
                var userDetails = userDAC.SaveCustomerDetails(user);
                return userDetails;
        }


       
        public bool AddTweet(Tweet obj )
        {
            if (obj.Message!=null && obj.UserId!=null)
            {
                userDAC.AddTweet(obj.Message, obj.UserId);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool EditTweet(Tweet obj)
        {
            if(obj.Message!=null && obj.TweetId>=0)
            {
                userDAC.EditTweet(obj.Message, obj.TweetId);
                return true;   
            }
            else
            {
                return false;
            }
        }


        public IList<Tweet> GetAllTweets(User userObj)
        {
            if (userObj.EmailId!=null) {

                return userDAC.GetAllTweets(userObj.EmailId);
              }
            else
            {
                return null;
            }
        }

        //Follow User

        public bool Follow(string currentUserId,string followeeUserId)
        {
            if(currentUserId!=null && followeeUserId != null)
            {
                userDAC.FollowUser(currentUserId, followeeUserId);
                return true;
            }
            else
            {
                return false;
            }
        }

        //getAll Follow 

        public IList<string> GetAllFollow(User userObj)
        {
            if (userObj!=null &&userObj.EmailId != null)
            {
                return userDAC.GetFollowers(userObj.EmailId);
            }
            else
            {
                return null;
            }
        }


        public IList<string> GetAllFollowee(User userObj)
        {
            if (userObj != null && userObj.EmailId != null)
            {
                return userDAC.GetFollowee(userObj.EmailId);
            }
            else
            {
                return null;
            }

        }
        
        public IList<User> GetAllTweetBasedOnHashTags(Hashtag HastagString)
        {
            if (HastagString != null && HastagString.TagName!=null)
            {
                return userDAC.SearchPosts(HastagString.TagName);
            }
            else
            {
                return null;
            }
        }


        public IList<User> GetAllTweetBasedOnPeople(User userObj)
        {
            if (userObj != null && userObj.Name!=null)
            {
                return userDAC.SearchPeople(userObj.Name);
            }
            else
            {
                return null;
            }
        }


        public bool LikeTweet(Tweet tweetObj)
        {
            if(tweetObj!=null && tweetObj.UserId!=null && tweetObj.TweetId>0)
            {
                userDAC.LikeTweet(tweetObj.TweetId, tweetObj.UserId);

                return true;
            }
            else
            {
                return false;
            }
        }


        public bool DisLikeTweet(Tweet tweetObj)
        {
            if (tweetObj != null && tweetObj.UserId != null && tweetObj.TweetId>0)
            {
                userDAC.DislikeTweet(tweetObj.TweetId, tweetObj.UserId);

                return true;
            }
            else
            {
                return false;
            }
        }


    }
}
