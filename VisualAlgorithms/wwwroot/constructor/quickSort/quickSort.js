if (window.innerWidth < window.innerHeight ) {
  alert("Пожалуйста, переверните экран телефона или расширьте окно для правильного отображения сайта и перезагрузите страницу");
}

let distance=75;
if(window.innerWidth<740){
 distance=60;
}

const blockSort = document.querySelector(".blockSort");
let places=[];
function generateNumbers() {
  
  for (let i = 0; i < 10; i ++) {
    const value = Math.floor(Math.random() * 100);

    const blockNumber = document.createElement("div");
    blockNumber.classList.add("element");
    blockNumber.style.transform = `translateX(${(i-4.5) * distance}px)`;
    places[i]=(i-4.5) * distance;
    blockNumber.innerHTML = value;
    blockSort.appendChild(blockNumber);
  }
}




generateNumbers();

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
        let clonedElement1 = element1.cloneNode(true);
        let clonedElement2 = element2.cloneNode(true);
        element2.parentNode.replaceChild(clonedElement1, element2);
        element1.parentNode.replaceChild(clonedElement2, element1);
        resolve();
      }, 2000);
    });
  });
}

let arr = document.querySelectorAll(".element");
btnSort.onclick=async function(){
  btnSort.style.visibility="hidden";
  await QuickSort(arr, 0, arr.length-1);
  let labels = document.querySelectorAll('.label');
  for(i=0; i<labels.length; i++) {
    labels[i].style.visibility="hidden";
  }
}

async function QuickSort(elements, begin, end) {
  elements = document.querySelectorAll(".element");
  let pivot =Number(elements[Math.floor((begin + end) / 2)].innerHTML);
  for(let i=0;i<elements.length;i++){
    if(elements[i].style.backgroundColor=="yellow"){
      elements[i].style.backgroundColor="#0094FF";
    }
  }

  elements[Math.floor((begin + end) / 2)].style.backgroundColor = "yellow";

  let labels = document.querySelectorAll('.label');
  for(i=0; i<labels.length; i++) {
    labels[i].parentNode.removeChild(labels[i]);
  }
  let left = begin;
      const leftlabel = document.createElement("div");
      leftlabel.classList.add("label");
      leftlabel.style.transform = `translate(${places[left]}px, 100px)`;
      leftlabel.innerHTML = "Left↑";
      blockSort.appendChild(leftlabel);

  let right = end;
      const rightlabel = document.createElement("div");
      rightlabel.classList.add("label");
      rightlabel.style.transform = `translate(${places[right]}px, 75px)`;
      rightlabel.innerHTML = "Right↑";
      blockSort.appendChild(rightlabel);


  while (left <= right){

      while (Number(elements[left].innerHTML) < pivot){       
          await new Promise(resolve =>
            setTimeout(() => {
              resolve();
            }, 1000)
          );
          left++;
          leftlabel.style.transform = `translate(${places[left]}px, 100px)`;
      }

      await new Promise(resolve =>
        setTimeout(() => {
          resolve();
        }, 2000)
      );

      if(left<=right){
        elements[left].style.backgroundColor="#FF0000";
      }

      while (Number(elements[right].innerHTML) > pivot){  
        
        await new Promise(resolve =>
          setTimeout(() => {
            resolve();
          }, 1000)
        );  
        right--; 
        rightlabel.style.transform = `translate(${places[right]}px, 75px)`;
      }
      
      if(left<right){
        elements[right].style.backgroundColor="#FF0000";
      }

      await new Promise(resolve =>
        setTimeout(() => {
          resolve();
        }, 2000)
      ); 

      
      let flag=false;

      if (left <= right){   
          if(left!=right){
            await swap(elements[left], elements[right]);
            elements = document.querySelectorAll(".element");
          }
                   
          flag=true;
          left++;
          right--;
          leftlabel.style.transform = `translate(${places[left]}px, 100px)`;
          rightlabel.style.transform = `translate(${places[right]}px, 75px)`;
          
      }

      if(flag){
        elements[left-1].style.backgroundColor="#0094FF";
        elements[right+1].style.backgroundColor="#0094FF";
        if(Number(elements[left-1].innerHTML)==pivot){
          elements[left-1].style.backgroundColor="yellow";
        }
        
        if(Number(elements[right+1].innerHTML)==pivot){
          elements[right+1].style.backgroundColor="yellow";
        }
      } else{
        elements[left].style.backgroundColor="#0094FF";
        elements[right].style.backgroundColor="#0094FF";
        if(Number(elements[left].innerHTML)==pivot){
          elements[left].style.backgroundColor="yellow";
        }
        
        if(Number(elements[right].innerHTML)==pivot){
          elements[right].style.backgroundColor="yellow";
        }
      }
      
  }

  
  if (begin < right){    
      await QuickSort(elements, begin, left - 1);
  }
  elements[begin].style.backgroundColor="#00DD21";
  elements[begin+1].style.backgroundColor="#00DD21";

  if (end > left){
      await QuickSort(elements, right + 1, end);
  }

  elements[right+1].style.backgroundColor="#00DD21";
  elements[end].style.backgroundColor="#00DD21";
}




