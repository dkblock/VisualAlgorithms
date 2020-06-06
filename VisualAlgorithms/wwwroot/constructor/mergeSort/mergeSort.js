if (window.innerWidth < window.innerHeight ) {
  alert("Пожалуйста, переверните экран телефона или расширьте окно для правильного отображения сайта и перезагрузите страницу");
}

let distance=75;
if(window.innerWidth<740){
 distance=60;
}

const blockSort = document.querySelector(".blockSort");
let places=[];
let colors=["lightsalmon", "orange", "yellow", "lightgreen", "limegreen", "cyan", "darkcyan", "dodgeblue", "magenta", "purple"];
function generateNumbers() {
  
  for (let i = 0; i < 10; i ++) {
    const value = Math.floor(Math.random() * 100);

    const blockNumber = document.createElement("div");
    blockNumber.classList.add("element");
    blockNumber.style.transform = `translateX(${(i-4.5) * distance}px)`;
    blockNumber.style.backgroundColor=colors[i];
    places[i]=(i-4.5) * distance;
    blockNumber.innerHTML = value;
    blockSort.appendChild(blockNumber);
  }
}



btnSort.onclick=async function mergeSort() {
  btnSort.style.visibility="hidden";
  let elements = document.querySelectorAll(".element");
  let step = 1;  
  
  while (step < elements.length)  
  {
    let countColors=0;
    let index = 0;    
    let left = 0;    
    let mid = left + step;
    let right = left + step * 2; 
    do
    {
      mid = (mid < elements.length) ? mid : elements.length; 
      right = (right < elements.length) ? right : elements.length;
             
      let i1 = left, i2 = mid; 
      for (; i1 < mid && i2 < right; ) 
      {
        let matrix1 = window.getComputedStyle(elements[i1]).transform;
        matrix1 = matrix1.split(/\(|,\s|\)/).slice(1,7);
        let matrix2 = window.getComputedStyle(elements[i2]).transform;
        matrix2 = matrix2.split(/\(|,\s|\)/).slice(1,7);

        if (Number(elements[i1].innerHTML) < Number(elements[i2].innerHTML)) {
           elements[i1].style.transform = `translate(${places[index]}px,-100px)`;
           elements[i1].style.backgroundColor=colors[countColors];
           await new Promise(resolve =>
           setTimeout(() => {
           resolve();
           }, 1000)
           );
           index++;
           i1++;
        } 
        else {
           elements[i2].style.transform = `translate(${places[index]}px,-100px)`;
           elements[i2].style.backgroundColor=colors[countColors];
           await new Promise(resolve =>
            setTimeout(() => {
            resolve();
            }, 1000)
            );
           index++;
           i2++;
        }
      }
     
      while (i1 < mid){ 
        let matrix1 = window.getComputedStyle(elements[i1]).transform;
        matrix1 = matrix1.split(/\(|,\s|\)/).slice(1,7);
        let matrix2 = window.getComputedStyle(elements[index]).transform;
        matrix2 = matrix2.split(/\(|,\s|\)/).slice(1,7);

        elements[i1].style.transform=`translate(${places[index]}px,-100px)`;
        elements[i1].style.backgroundColor=colors[countColors];
        await new Promise(resolve =>
          setTimeout(() => {
          resolve();
          }, 1000)
          );
        index++;
        i1++;
      } 
      while (i2 < right){
        let matrix1 = window.getComputedStyle(elements[i1]).transform;
        matrix1 = matrix1.split(/\(|,\s|\)/).slice(1,7);
        let matrix2 = window.getComputedStyle(elements[index]).transform;
        matrix2 = matrix2.split(/\(|,\s|\)/).slice(1,7);

        elements[i2].style.transform=`translate(${places[index]}px,-100px)`;
        elements[i2].style.backgroundColor=colors[countColors];
        await new Promise(resolve =>
          setTimeout(() => {
          resolve();
          }, 1000)
          );
        index++;
        i2++;
      }
      sorted(elements,left,right);
      countColors++;
      elements=document.querySelectorAll(".element");
      for  (let i = left; i < right; i++){ 
        elements[i].style.transform=`translate(${places[i]}px, 0px)`;
        await new Promise(resolve =>
          setTimeout(() => {
          resolve();
          }, 1000)
          );
      }
      left += step * 2; 
      mid += step * 2;
      right += step * 2;
    } while (left < elements.length); 
    
    step *= 2; 
  }
}
 

function sorted(arr,left,right){
  for (let i = left; i < right - 1; i ++) {
    for (let j = left; j < right - 1; j ++) {
      let matrix1 = window.getComputedStyle(arr[j]).transform;
      matrix1 = matrix1.split(/\(|,\s|\)/).slice(1,7);
      let matrix2 = window.getComputedStyle(arr[j+1]).transform;
      matrix2 = matrix2.split(/\(|,\s|\)/).slice(1,7);

      const value1 = Number(matrix1[4]);
      const value2 = Number(matrix2[4]);

      if (value1 > value2) {
        blockSort.insertBefore(arr[j+1], arr[j]);
        arr = document.querySelectorAll(".element");
      }
    }
  }
}

generateNumbers();
