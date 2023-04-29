EXTERNAL EnableSwiping()
EXTERNAL DisableSwiping()
EXTERNAL AddTimer()
EXTERNAL ChangeSpeakerCardLeft()
EXTERNAL ChangeSpeakerCardRight()
EXTERNAL ScreenShake()

->tutorial

=== tutorial ===
No! I'm innocent, I tell you! #speaker:Gorg
Bailiff! Take him away immediately unless he wishes to face charges of court misconduct. #speaker:You
But I didn’t do it! #speaker:Gorg
~ ChangeSpeakerCardLeft()
The court is now adjourned. #speaker:You
… #speaker: #sfx:0
Gosh, I hope the prosecution gets me a fat paycheck for all the extra work they put me through… #speaker:You
~ ChangeSpeakerCardRight()
Are you not ashamed of yourself? #speaker:???
You call yourself a judge, yet all you do is take bribes. #speaker:???
(Wait, what?) #speaker:You
(First, I have the worst munchies, and now I start seeing things.)
Uhm… Are you a talking gavel?
I am not a "talking gavel". My name is Václav, Václav The Gavel. #speaker:Václav
Perhaps you know me better as the Spirit of Justice.
I have come to stop your corrupt ways!
By doing what, exactly? Speaking in a funny voice? #speaker:You
Silence! #speaker:Václav
~ ScreenShake()
… #speaker: #sfx:1
Eek! #speaker:You
I sentence you to sit in this exact courtroom and solve cases until you repent from the bottom of your heart! #speaker:Václav
(Is this really all the punishment?) #speaker:You
No, it is not. #speaker:Václav
Hey, who said you can read my thoughts? #speaker:You
'Tis but one of my powers. #speaker:Václav
Let us add some challenge. Do not pass a false verdict. #speaker:Václav
Not that I ever did. #speaker:You
Of course, of course. #speaker:Václav
I see that you are still a long way from true repentance. #speaker:Václav
I shall add a timer as well. #speaker:Václav
~ AddTimer()
(What did I get myself into…) #speaker:You
While we are at it, let us review your last defendant's verdict. #speaker:Václav
Analyze their case properly and then swipe their profile <b>left</b> or <b>right</b>. Your kind must be familliar with this.
-> choice

=== choice ===
~ ChangeSpeakerCardRight()
~ EnableSwiping()
    + [left]
        ~ DisableSwiping()
        Are you sure you made the right choice? #speaker:Václav
        Let us try again.
        -> choice
    + [right]
        ~ DisableSwiping()
        At last! #speaker:Václav
-> finale

=== finale ===
See? Doesn't it feel good to pass a proper verdict? #speaker:Václav
Now you can finally get on the path to redemption.
Who knows, perhaps one day you shall get freed from this courtroom.
->END