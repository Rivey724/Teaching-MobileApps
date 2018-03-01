Raymond Ivey 

Post morten for PA3


PA3 was in my opinion was maybe the most challenging assingment for me so far, though PA2 may have 
been a bit more difficult. For me, the part that took the most time was the coding in a proper
countdown timer, then figuring out how to get it to display properly on the screen. My first attempt
involved a timer class, however I soon discovered that it could only either count up, or could only 
count down once, and then wouldn't count anymore. Its something I need to look into more in the future.
Eventually I discovered a Java CountDown timer that exists in Xamarin C# for Android, however that
apprently doesn't actually work too well. The night the project was due, Tuesday, I found a better
way to do with by using a System timer, and counting that down. In order to update the timer
on the UI through a textview, I had to use a very strange piece of code called RunOnUIThread, that
I discovered could also preform tasks such as updating a timer on the screen as it counts down or up,
as well as actively change the layout and set up buttons and imageviews as well. I would say
coding the timer took the majority of the time, as working with the API got quite annoying after
multiple test runs. I discovered during testing that it could not recongize certain objects such
as scissors, which I tested on the app as well as through the Google Vision site. I also
discovered that if the user  took a pixelated image, it would have trouble recognizing what the
picture was. This is something i'll have to work with and fix in the future. Another idea I had
to go further with this was to create leaderboards and accounts that could be handled by a database,
with users creating accounts and being able to race against their friends to see who could
complete a series of 5 challenges the fastest. You could also change up the items that the app
has every week or so in order to change it up. Of course, this would take quite a bit of time to code,
but its something I may attempt over the summer. 