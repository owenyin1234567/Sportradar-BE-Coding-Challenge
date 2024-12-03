
    var c = document.getElementById("mySidebar");
    var MenuSidebarIsClosed = true;
    var ProfileSidebarIsClosed = true;
        
    // Set the date we're counting down to
    var SecondsToSessionTimeput = 900;
    var CountSeconds = 0; 


    document.onreadystatechange = function (e) {
        if (document.readyState === 'complete') {
            document.getElementById("mySidebar");
            MenuSidebarIsClosed = true;
            ProfileSidebarIsClosed = true;
            SecondsToSessionTimeput = 900;
            CountSeconds = 0;
        }
    }
        
        function opencloseNav() {
            //console.log("function opencloseNav");

            MenuSidebarIsClosed = !MenuSidebarIsClosed;  
            document.getElementById("Sidebar-Background").classList.toggle('hidden',MenuSidebarIsClosed);
            document.getElementById("mySidebar").classList.toggle('hidden',MenuSidebarIsClosed);
        }
        
        function opencloseProfilePicture()
        {
            //console.log("function opencloseProfilePicture");

            if(ProfileSidebarIsClosed===true){
                document.getElementById("Profile-PictureOverviewSidebar").style.display="block";
                //console.log("Style Display: "+document.getElementById("Profile-PictureOverviewSidebar").style.display);
                //console.log("ProfileSidebarIsClosed: "+ProfileSidebarIsClosed);
                ProfileSidebarIsClosed = false;
            }
            else if(ProfileSidebarIsClosed===false) 
            {
                document.getElementById("Profile-PictureOverviewSidebar").style.display="none";
                //console.log("Style Display: "+document.getElementById("Profile-PictureOverviewSidebar").style.display);
                //console.log("ProfileSidebarIsClosed: "+ProfileSidebarIsClosed);
                ProfileSidebarIsClosed = true;
            }
        }   


function SessionTimer()
{
    //console.log("Start function SessionTimer")    

    // Update the count down every 1 second
    var x = setInterval(function() {

    
    // Get today's date and time
    CountSeconds = CountSeconds+1;
        
    // Find the distance between now and the count down date
    var distance = SecondsToSessionTimeput - CountSeconds;
        
    //console.log("distance: ",distance);
    //console.log("SecondsToSessionTimeput: ",SecondsToSessionTimeput);
    //console.log("CountSeconds: ",CountSeconds);


    // Time calculations for days, hours, minutes and seconds
    var minutes = Math.floor(distance/60);
    var seconds = Math.floor(distance % 60);
        
    // Output the result in an element with id="demo"
    document.getElementById("Session-Timer").innerHTML = "Timeout in: " + minutes + " M  " + seconds + "S ";
        
    // If the count down is over, write some text 
    if (distance < 0) {
        clearInterval(x);
        document.getElementById("Session-Timer").innerHTML = "SESSION EXPIRED";
        Logout();
    }

    },1000); //1000 sollte beim release sein
}

function ResetTimer()
{
    //console.log("Reset Timer");
    SecondsToSessionTimeput = 901;
    CountSeconds = 0; 
    //console.log("ResetTimer Add15MinToApiKeyExpDate");
    Add15MinToApiKeyExpDate();
    //console.log("ResetTimer Add15MinToApiKeyExpDate");
    //Add15MinToApiKeyExpDate();

}

function LoadSidebarwithCoins()
{
    //console.log("function LoadSidebarwithCoins start");
    
    var allCoins = GetAllCoinsfromString(GetCookie("FollowedCoins"));

    //console.log("allCoins ",allCoins);

    allCoins.forEach(Coin => {

        if(Coin != "")
        {
          const node = document.createElement("a");
          node.id = Coin;

          // Create a text node:
          node.innerHTML = Coin;

          //verlinkt die Website beim erstellen der Node auf die entsprechende Coin
          node.href = "https://cryptoxchange22.web.app/html/ViewData?Coin="+Coin;

          //console.log("https://cryptoxchange22.web.app/html/ViewData?Coin=+",Coin)
          //console.log("node",node);

          // Append the text node to the node:
          var mySidebar = document.getElementById("mySidebar")
          mySidebar.appendChild(node);
        }                
      });
}

