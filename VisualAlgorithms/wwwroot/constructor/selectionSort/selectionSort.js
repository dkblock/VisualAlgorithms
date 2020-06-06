if (window.innerWidth < window.innerHeight ) {
  alert("Пожалуйста, переверните экран телефона или расширьте окно для правильного отображения сайта и перезагрузите страницу");
}

let distance=75;
if(window.innerWidth<740){
 distance=60;
}

const blockSort = document.querySelector(".blockSort");

function generateNumbers() {
  
  for (let i = 0; i < 10; i ++) {
    const value = Math.floor(Math.random() * 100);

    const blockNumber = document.createElement("div");
    blockNumber.classList.add("element");
    blockNumber.style.transform = `translateX(${(i-4.5) * distance}px)`;

    blockNumber.innerHTML = value;
    blockSort.appendChild(blockNumber);
  }
}

function swap(element1, element2) {
  return new Promise(resolve => {
    const style1 = window.getComputedStyle(element1);
    const style2 = window.getComputedStyle(element2);

    
    const transform1 = style1.getPropertyValue("transform");
    const transform2 = style2.getPropertyValue("transform");
    
    element1.style.transform = transform2;
    element2.style.transform = transform1;
     
    window.requestAnimationFrame(function() {
      setTimeout(() => {
        var clonedElement1 = element1.cloneNode(true);
        var clonedElement2 = element2.cloneNode(true);

        element2.parentNode.replaceChild(clonedElement1, element2);
        element1.parentNode.replaceChild(clonedElement2, element1);
        clonedElement1.style.backgroundColor = "#0094FF"; 
        resolve();
      }, 2000);
    });
  });
}

btnSort.onclick=async function selectionSort() {
  btnSort.style.visibility="hidden";
  let elements = document.querySelectorAll(".element");
  for (let i = 0; i < elements.length - 1; i++) {
    let min = i;
    elements[i].style.backgroundColor = "#FF0000";
    for (let j = i+1; j < elements.length; j++) {
      elements[j].style.backgroundColor = "#FFD800";
      
      await new Promise(resolve =>
        setTimeout(() => {
          resolve();
        }, 1000)
      );

      const value1 = Number(elements[j].innerHTML);
      const value2 = Number(elements[min].innerHTML);

      if (value1 < value2) { 
        elements[min].style.backgroundColor = "#0094FF";  
        elements[j].style.backgroundColor = "#FF0000";   
        min=j;       
      } else{ elements[j].style.backgroundColor = "#0094FF";}
      
    }
    elements[i].style.backgroundColor = "#FF0000"; 
    if(i!=min){
      await new Promise(resolve =>
        setTimeout(() => {
          resolve();
        }, 1000)
      );
      await swap(elements[i], elements[min]);
    }
    elements = document.querySelectorAll(".element");
    elements[i].style.backgroundColor = "#00DD21";
  }
  elements[elements.length-1].style.backgroundColor = "#00DD21";
}

generateNumbers();
