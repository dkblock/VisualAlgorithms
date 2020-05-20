const blockSort = document.querySelector(".blockSort");

function generateNumbers() {
  
  for (let i = 0; i < 10; i ++) {
    const value = Math.floor(Math.random() * 100);

    const blockNumber = document.createElement("div");
    blockNumber.classList.add("element");
    blockNumber.style.transform = `translateX(${(i-4.5) * 75}px)`;

    blockNumber.innerHTML = value;
    blockSort.appendChild(blockNumber);
  }
}

function swap(element1, element2,element3) {
  return new Promise(resolve => {
    const style1 = window.getComputedStyle(element1);
    const style2 = window.getComputedStyle(element2);

    
    const transform1 = style1.getPropertyValue("transform");
    const transform2 = style2.getPropertyValue("transform");
    
    element1.style.transform = transform2;
    element2.style.transform = transform1;

    window.requestAnimationFrame(function() {
      setTimeout(() => {
        blockSort.insertBefore(element2, element1);
        blockSort.insertBefore(element1, element3);
        resolve();
      }, 2000);
    });
  });
}

btnSort.onclick=async function bubbleSort() {
             
  let elements = document.querySelectorAll(".element");
  for (let i = 0; i < elements.length - 1; i++) {
    let min = i;
    elements[i].style.backgroundColor = "red";
    for (let j = i+1; j < elements.length; j++) {
      elements[j].style.backgroundColor = "yellow";
      // elements[j + 1].style.backgroundColor = "red";
      
      await new Promise(resolve =>
        setTimeout(() => {
          resolve();
        }, 2000)
      );

      const value1 = Number(elements[j].innerHTML);
      const value2 = Number(elements[min].innerHTML);

      if (value1 < value2) { 
        elements[min].style.backgroundColor = "deepskyblue";  
        elements[j].style.backgroundColor = "red";   
        min=j;       
      } else{ elements[j].style.backgroundColor = "deepskyblue";}
      
    }
    elements[i].style.backgroundColor = "red";  
    await swap(elements[i], elements[min], elements[min+1]);
    elements[i].style.backgroundColor = "deepskyblue"; 
    elements = document.querySelectorAll(".element");
    elements[i].style.backgroundColor = "lightgreen";
  }
  elements[elements.length-1].style.backgroundColor = "lightgreen";
}

generateNumbers();
