// console.log("HI");
// let hrElement;
// let counter = 100;
// for (let i = 0; i < counter; i++) {
//     hrElement = document.createElement("HR");
//     hrElement.style.left = Math.floor(Math.random() * window.innerWidth) + "px";
//     hrElement.style.animationDuration = 0.2 + Math.random() * 0.3 + "s";
//     hrElement.style.animationDelay = Math.random() * 5 + "s";
//     console.log(hrElement);
//     document.body.appendChild(hrElement);
//   }
// }

function myFunction() {
  var x = document.getElementById("myTopnav");
  if (x.className === "topnav") {
    x.className += " responsive";
  } else {
    x.className = "topnav";
  }
}