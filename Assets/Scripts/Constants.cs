using System.Collections.Generic;

public class Constants{
    
    public static List<string> WelcomeMessages = new List<string>{
        "Welcome to NavBot! Let's get started!",
        "Hello! NavBot is ready to assist you!",
        "Greetings! Choose your destination with NavBot!",
        "Welcome to NavBot! Select where you want to go!",
        "Hi there! NavBot is at your service. Pick a destination!"
    };
    public static string Introduction = "I can navigate to your desired destination. For now, I can navigate only from 212 to 214. You can say 'GOTO 212'. Apart from that You can ask me questions about this building by saying 'Hey Luna!'. I'm currently trained on few data, so my knowledge is restricted - I'll try my best to answer your question";
    public static List<string> DestinationMessages = new List<string>{
        "Destination Reached!",
        "You've Arrived!",
        "Welcome to Your Destination!",
        "You Made It!",
        "Congratulations! You're Here!"
    };
    public static List<string> FollowMeMessages = new List<string>{
        "Please follow me!",
        "Come along with me, please.",
        "Follow my lead, if you would.",
        "Let's go together, follow my steps.",
        "I'll be your guide, follow my direction.",
        "Join me on this path, follow closely.",
    };
    public static List<string> WaitingMessages = new List<string>{
        "Could you please keep up with me!",
        "Let's pick up the pace, shall we?",
        "Are you able to keep up with the speed?",
        "Stay with me, we're almost there!",
        "Is everything going smoothly for you?",
        "Are you doing okay? We can take a break if needed.",
    };
}
