# App.Solution

1) Controllers
- AuthController
POST: login (email, password) 
POST: signup (name, email, password) 
GET: get-user/{email} 
POST: send-email/{type}
      -> Note: type is equal to "reset-password" or "confirm-email" 
PATCH: reset-password (code, new, confirm) 

[Authorized] 
- UsersController
GET: all
      -> Note: query paramters with pagination settings
GET: {userEmail}
PATCH: personal-data
PATCH: profile (image) 
DELETE: profile
PATCH: cover (image) 
DELETE: cover
PATCH: change-password (current, new, confirm) 
## to be continued (if necessary)... 

- PostsController
GET: all
      -> Note: query paramters with pagination settings
GET: {postId} 
POST: add (caption, list of images) 
      -> Note: Authorized
PATCH: update (postId, caption, list of images) 
      -> Note: Authorized
POST: like/{postId} 
      -> Note: Auth, toggler
DELETE: {postId} 
      -> Note: Authorized

- RequestsController
GET: all
      -> Note: Authorized
DELETE: reject/{requestId} 
PUT: accept/{requestId} 
      -> Note: Authorized, when request accepted it's deleted and added in mentors table

[Authorized] 
- SkillsController
GET: all
      -> Note: query paramters with pagination settings
POST: add (title, image?) 
DELETE: {skillId} 

[Authorized] 
- ExperienceController
GET: all
      -> Note: query paramters with pagination settings

[Authorized] 
- RatesController
POST: add (mentorId, count, comment) 
GET: all
      -> Note: query paramters with pagination settings

[Authorized] 
- TimelineController
GET: discover
GET: following

## Still thinking (View depend on role if mentor view his students, if student view his mentors) 
[Authorized] 
- ContactsController
GET: all
      -> Note: query paramters with pagination settings
DELETE: {mentorId}
      -> Note: Authorized("student")  