# TwitterClone Assessment
## Assumptions
1)	The application requires all exact files explicitly to run
2)	The files that are passed through as arguments will have the same formatting to be able to compile the data
3)	Files cannot contain no content
4)	The programming stores the data in memory before outputting to the console output
5)	The files act as a storage place for the data to be inputted in the app
6)	A user is also a follower
7)	A user can allow multiple users to follow them (including following themselves to receive posts)
8)	A user can see all messages sent by themselves and people they are following
9)	A user must exist in the users file to be able to send a post
10)	A post cannot be empty but there are no current character limit restrictions
11)	Posts are sent to followers in the order of the tweet file
12)	The “timeline” is ordered by users alphabetically and then by messages from people they follow
13)	Users cannot unfollow yet

## Design Patterns
I originally just made the program to read in files and output the files to console.
I also used a simple version of the Observer pattern and the PubSub pattern for managing users and posts but it felt a little redundant as there is no user interaction but rather a play back from the text files. I’ve attached both files in the solution as CS regular files (PubSub.cs and Observable.cs)