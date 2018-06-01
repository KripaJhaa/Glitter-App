using DAL;
using ModelsDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Data.Entity;

namespace DataAccess
{
    public class UserDAC
    {
        public BlogDbContext db = new BlogDbContext();

        public bool IsValidUser(User user)
        {
            var newUser = db.Users.Where(a => a.EmailId.Equals(user.EmailId) && a.Password.Equals(user.Password)).FirstOrDefault();
            if (newUser != null)
            {
                
                return true;
            }

            else
            {
                return false;
            }
        }

        public bool IsUserExist(User user)
        {
            var newUser = db.Users.FirstOrDefault(p => p.EmailId == user.EmailId);

            if (newUser != null)
                return true;
            else
                return false;
        }

        public User SaveCustomerDetails(User user)
        {
                    User newuser = new User
                    {
                        Name = user.Name,
                        EmailId = user.EmailId,
                        PhoneNumber = user.PhoneNumber,
                        Password = user.Password,
                        CountryOfOrigin = user.CountryOfOrigin,
                        Image = user.Image
                    };
                    db.Users.Add(newuser);
                    db.SaveChanges();
                    return newuser;
        }


        //----------------------

        // Add Tweet By User

        public void AddTweet(string message, string userId)
        {

            try
            {
                using (BlogDbContext context = new BlogDbContext())
                {
                    Tweet tweet = new Tweet
                    {
                        Message = message,
                        UserId = userId,
                        Date = DateTime.Now
                    };
                    context.Tweets.Add(tweet);
                    context.SaveChanges();
                }
                AddHashtag(message, userId);

            }
            catch (NullReferenceException)
            {
                //log for errors
                //Console.WriteLine(e);
            }

        }

        //Add HashTag

        private void AddHashtag(string message, string userId)
        {

            try
            {
                using (BlogDbContext context = new BlogDbContext())
                {
                    var regex = new Regex(@"(?<=#)\w+");
                    var matches = regex.Matches(message);

                    foreach (Match m in matches)
                    {
                        var hashtag = context.Hashtags.Where(x => x.UserId == userId && x.TagName == m.Value).FirstOrDefault();
                        if (hashtag != null)
                        {
                            hashtag.Count++;
                            context.Entry(hashtag).State = EntityState.Modified;
                            context.SaveChanges();
                        }
                        else
                        {
                            Hashtag hash = new Hashtag
                            {
                                TagName = m.Value,
                                UserId = userId,
                                Count = 1
                            };
                            context.Hashtags.Add(hash);
                            context.SaveChanges();
                        }
                    }

                }

            }
            catch (NullReferenceException)
            {
                //log for errors
                //Console.WriteLine(e);
            }
        }

        //EditTweet

        public void EditTweet(string message, int TweetId)
        {

            try
            {

                using (BlogDbContext context = new BlogDbContext())
                {
                    var tweet = context.Tweets.Where(x => x.TweetId == TweetId).FirstOrDefault();
                    if (tweet != null)
                    {
                        RemoveHashtags(tweet.Message, tweet.UserId);
                        tweet.Message = message;
                        tweet.Date = DateTime.Now;
                        context.Entry(tweet).State = EntityState.Modified;
                        context.SaveChanges();
                        AddHashtag(message, tweet.UserId);
                    }
                    else
                    {

                    }
                }


            }
            catch (NullReferenceException)
            {
                //log for errors
                //Console.WriteLine(e);
            }

        }

        //Remove HashTags

        private void RemoveHashtags(string message, string userId)
        {

            try
            {
                using (BlogDbContext context = new BlogDbContext())
                {
                    var regex = new Regex(@"(?<=#)\w+");
                    var matches = regex.Matches(message);

                    foreach (Match m in matches)
                    {
                        var hashtag = context.Hashtags.Where(x => x.UserId == userId && x.TagName == m.Value).FirstOrDefault();
                        if (hashtag != null)
                        {
                            hashtag.Count--;
                            if (hashtag.Count == 0)
                            {
                                context.Entry(hashtag).State = EntityState.Deleted;

                            }
                            else
                            {
                                context.Entry(hashtag).State = EntityState.Modified;
                            }
                            context.SaveChanges();

                        }
                        else
                        {
                            //log for errors
                        }
                    }

                }

            }
            catch (NullReferenceException)
            {
                //log for errors
                //Console.WriteLine(e);
            }
        }


        //Get All Tweets of the User

        public IList<Tweet> GetAllTweets(string userId)
        {

            IList<Tweet> returnedTweets = null;
            try
            {
                using (BlogDbContext context = new BlogDbContext())
                {
                    IList<string> users = GetFollowee(userId);
                    users.Add(userId);
                    var tweets = (from p in context.Tweets
                                  where users.Contains(p.UserId)
                                  orderby p.Date descending
                                  select p).ToList();

                    if (tweets != null)
                    {
                        returnedTweets = new List<Tweet>();
                        foreach (var tweet in tweets)
                        {
                            Tweet returnedTweet = new Tweet
                            {
                                TweetId = tweet.TweetId,
                                Message = tweet.Message,
                                UserId = tweet.UserId,
                                Date = tweet.Date
                            };
                            returnedTweets.Add(returnedTweet);
                        }

                    }

                }
            }
            catch (NullReferenceException)
            {
                returnedTweets = null;
            }

            return returnedTweets;
        }

        //func used by get all tweet   AND      to Find Followee

        public IList<string> GetFollowingUsers(string userId)
        {

            IList<string> returnedUsers = null;
            try
            {
                using (BlogDbContext context = new BlogDbContext())
                {

                    var users = (from p in context.UserLinks
                                 select p).Where(x => x.FolloweeId.Equals(userId)).ToList();

                    if (users != null)
                    {
                        returnedUsers = new List<string>();
                        foreach (var user in users)
                        {
                            returnedUsers.Add(user.FollowerId);
                        }

                    }

                }
            }
            catch (NullReferenceException)
            {
                returnedUsers = null;
            }

            return returnedUsers;
        }


        // Follow User
        public void FollowUser(string currentUserId, string followingUserId)
        {

            try
            {
                using (BlogDbContext context = new BlogDbContext())
                {

                    var followEntry = (from p in context.UserLinks
                                       select p).Where(x => x.FolloweeId.Equals(currentUserId) && x.FollowerId.Equals(followingUserId)).FirstOrDefault();

                    if (followEntry != null)
                    {

                        context.Entry(followEntry).State = EntityState.Deleted;
                        context.SaveChanges();
                    }

                    else
                    {
                        UserLink returnedfollowEntry = new UserLink
                        {
                            FolloweeId = currentUserId,
                            FollowerId = followingUserId,
                        };
                        context.UserLinks.Add(returnedfollowEntry);
                        context.SaveChanges();
                    }

                }
            }
            catch (NullReferenceException)
            {

            }
        }


        //>>>>>>>>>>>>>>>>>>>

        public IList<string> GetFollowers(string userId)
        {

            IList<string> returnedUsers = null;
            try
            {
                using (BlogDbContext context = new BlogDbContext())
                {

                    var users = (from p in context.UserLinks
                                 select p).Where(x => x.FolloweeId.Equals(userId)).ToList();

                    if (users != null)
                    {
                        returnedUsers = new List<string>();
                        foreach (var user in users)
                        {
                            returnedUsers.Add(user.FollowerId);
                        }

                    }

                }
            }
            catch (NullReferenceException)
            {
                returnedUsers = null;
            }

            return returnedUsers;
        }

        //  Get Followee
        public IList<string> GetFollowee(string userId)
        {

            IList<string> returnedUsers = null;
            try
            {
                using (BlogDbContext context = new BlogDbContext())
                {

                    var users = (from p in context.UserLinks
                                 select p).Where(x => x.FollowerId.Equals(userId)).ToList();

                    if (users != null)
                    {
                        returnedUsers = new List<string>();
                        foreach (var user in users)
                        {
                            returnedUsers.Add(user.FolloweeId);
                        }

                    }

                }
            }
            catch (NullReferenceException)
            {
                returnedUsers = null;
            }

            return returnedUsers;
        }

        //------------------------------------------------------- Search On Basis Of HashTags

        public IList<User> SearchPosts(string searchString)
        {

            IList<User> returnUsers = null;
            try
            {
                using (BlogDbContext context = new BlogDbContext())
                {

                    var Hashtags = (from p in context.Hashtags
                                    select p).Where(x => x.TagName.Contains(searchString)).ToList();

                    IList<string> userIds = new List<string>();
                    foreach (var hashtag in Hashtags)
                    {
                        userIds.Add(hashtag.UserId);
                    }

                    var users = (from p in context.Users
                                 where userIds.Contains(p.EmailId)
                                 select p).ToList();

                    if (users != null)
                    {
                        returnUsers = new List<User>();
                        foreach (var user in users)
                        {
                            User returnUser = new User
                            {
                                EmailId = user.EmailId,
                                Name = user.Name,
                                CountryOfOrigin = user.CountryOfOrigin,
                                Image = user.Image,
                                PhoneNumber = user.PhoneNumber,
                                Password = ""
                            };
                            returnUsers.Add(returnUser);
                        }

                        return returnUsers;

                    }
                    else
                    {
                        return null;
                    }

                }
            }
            catch(Exception e)
            {
                return null;
            }    

            }

        //-------------- Search Tweets Based on People

        public IList<User> SearchPeople(string searchString)
        {

            IList<User> returnUsers = null;
            try
            {
                using (BlogDbContext context = new BlogDbContext())
                {

                    var users = (from p in context.Users
                                 select p).Where(x => x.EmailId.Contains(searchString) || x.Name.Contains(searchString)).ToList();

                    if (users != null)
                    {
                        returnUsers = new List<User>();
                        foreach (var user in users)
                        {
                            User returnUser = new User
                            {
                                EmailId = user.EmailId,
                                Name = user.Name,
                                CountryOfOrigin = user.CountryOfOrigin,
                                Image = user.Image,
                                PhoneNumber = user.PhoneNumber,
                                Password = ""
                            };
                            returnUsers.Add(returnUser);
                        }

                    }

                }
            }
            catch (NullReferenceException)
            {
                returnUsers = null;
            }

            return returnUsers;

        }





        // Like Tweet

        public void LikeTweet(int TweetId, string userId)
        {

            try
            {
                using (BlogDbContext context = new BlogDbContext())
                {
                    var likedTweet = context.Likes.Where(x => x.UserId == userId && x.TweetId == TweetId).FirstOrDefault();
                    if (likedTweet != null)
                    {
                        if (likedTweet.IsLiked == true)
                        {
                            context.Entry(likedTweet).State = EntityState.Deleted;
                            context.SaveChanges();
                        }
                        else
                        {
                            likedTweet.IsLiked = true;
                            context.Entry(likedTweet).State = EntityState.Modified;
                            context.SaveChanges();
                        }

                    }
                    else
                    {
                        Like like = new Like
                        {
                            TweetId = TweetId,
                            UserId = userId,
                            IsLiked = true
                        };
                        context.Likes.Add(like);
                        context.SaveChanges();
                    }

                }

            }
            catch (NullReferenceException)
            {
                //log for errors
                //Console.WriteLine(e);
            }

        }


        //Dislike Tweet

        public void DislikeTweet(int TweetId, string userId)
        {

            try
            {
                using (BlogDbContext context = new BlogDbContext())
                {
                    var likedTweet = context.Likes.Where(x => x.UserId == userId && x.TweetId == TweetId).FirstOrDefault();
                    if (likedTweet != null)
                    {
                        if (likedTweet.IsLiked == false)
                        {
                            context.Entry(likedTweet).State = EntityState.Deleted;
                            context.SaveChanges();
                        }
                        else
                        {
                            likedTweet.IsLiked = false;
                            context.Entry(likedTweet).State = EntityState.Modified;
                            context.SaveChanges();
                        }

                    }
                    else
                    {
                        Like like = new Like
                        {
                            TweetId = TweetId,
                            UserId = userId,
                            IsLiked = false
                        };
                        context.Likes.Add(like);
                        context.SaveChanges();
                    }

                }

            }
            catch (NullReferenceException)
            {
                //log for errors
                //Console.WriteLine(e);
            }

        }



    }
}
