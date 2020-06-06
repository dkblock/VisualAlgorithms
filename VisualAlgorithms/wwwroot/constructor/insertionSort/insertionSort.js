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
    let matrix1 = window.getComputedStyle(element1).transform;
    matrix1 = matrix1.split(/\(|,\s|\)/).slice(1,7);

    let matrix2 = window.getComputedStyle(element2).transform;
    matrix2 = matrix2.split(/\(|,\s|\)/).slice(1,7);

    element1.style.transform = `translate(${matrix2[4]}px, 0px)`;

    element2.style.transform = `translate(${matrix1[4]}px, ${matrix2[5]}px)`;

    window.requestAnimationFrame(function() {
      setTimeout(() => {
        blockSort.insertBefore(element2, element1);
        resolve();
      }, 1000);
    });
  });
}

btnSort.onclick=async function insertionSort() {
  btnSort.style.visibility="hidden";

  let elements = document.querySelectorAll(".element");
  elements[0].style.backgroundColor = "#00DD21";

  await new Promise(resolve =>
          setTimeout(() => {
            resolve();
          }, 2000)
        );

  for (let i = 1; i < elements.length ; i ++) {
    let matrix = window.getComputedStyle(elements[i]).transform;
    matrix = matrix.split(/\(|,\s|\)/).slice(1,7);

    elements[i].style.backgroundColor = "#FF0000";
    elements[i].style.transform = `translate(${matrix[4]}px,-100px)`;
    elements[i-1].style.backgroundColor = "#FFD800";
    let count=i;
    let flag=false;
    for (let j = i; j > 0 && Number(elements[j-1].innerHTML)>Number(elements[j].innerHTML); j --) {

      elements[j-1].style.backgroundColor = "#FFD800";
      await new Promise(resolve =>
        setTimeout(() => {
          resolve();
        }, 1000)
      );

      const value1 = Number(elements[j-1].innerHTML);
      const value2 = Number(elements[j].innerHTML);

      if (value1 > value2) {
        await swap(elements[j-1], elements[j]);
        elements = document.querySelectorAll(".element");
      }
      count=j;
      elements[j].style.backgroundColor = "#00DD21";
      flag=true;
    }

    if(flag){
      matrix = window.getComputedStyle(elements[count-1]).transform;
      matrix = matrix.split(/\(|,\s|\)/).slice(1,7);
      elements[count-1].style.transform = `translate(${matrix[4]}px,0px)`;
    } else{
      matrix = window.getComputedStyle(elements[count]).transform;
      matrix = matrix.split(/\(|,\s|\)/).slice(1,7);
      elements[count].style.transform = `translate(${matrix[4]}px,0px)`;
    }
    
    elements[count].style.backgroundColor = "#00DD21";
    elements[count-1].style.backgroundColor = "#00DD21";
  }
  matrix = window.getComputedStyle(elements[count-1]).transform;
  matrix = matrix.split(/\(|,\s|\)/).slice(1,7);
  elements[elements.length-1].style.transform = `translate(${matrix[4]}px,0px)`;
}

generateNumbers();
