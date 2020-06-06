if (window.innerWidth < window.innerHeight ) {
   alert("Пожалуйста, переверните экран телефона или расширьте окно для правильного отображения сайта и перезагрузите страницу");
}

let distance=75;
if(window.innerWidth<740){
  distance=60;
}

const blockSort = document.querySelector(".blockSort");

function generateNumbers() {
  for (let i = 0; i < 10; i++) {
    const value = Math.floor(Math.random() * 100);

    const blockNumber = document.createElement("div");
    blockNumber.classList.add("element");
    blockNumber.style.transform = `translateX(${(i-4.5) * distance}px)`;

    blockNumber.innerHTML = value;
    blockSort.appendChild(blockNumber);
  }
}

function swap(element1, element2) {
  return new Promise((resolve) => {
    const style1 = window.getComputedStyle(element1);
    const style2 = window.getComputedStyle(element2);

    const transform1 = style1.getPropertyValue("transform");
    const transform2 = style2.getPropertyValue("transform");

    element1.style.transform = transform2;
    element2.style.transform = transform1;

    window.requestAnimationFrame(function () {
      setTimeout(() => {
        blockSort.insertBefore(element2, element1);
        resolve();
      }, 1000);
    });
  });
}

btnSort.onclick = async function bubbleSort() {
  btnSort.style.visibility="hidden";
  document.getElementById(btnSort)
  let elements = document.querySelectorAll(".element");
  for (let i = 0; i < elements.length - 1; i ++) {
    for (let j = 0; j < elements.length - i - 1; j ++) {
      elements[j].style.backgroundColor = "#FF0000";
      elements[j + 1].style.backgroundColor = "#FF0000";

      await new Promise((resolve) =>
        setTimeout(() => {
          resolve();
        }, 1000)
      );

      const value1 = Number(elements[j].innerHTML);
      const value2 = Number(elements[j + 1].innerHTML);

      if (value1 > value2) {
        await swap(elements[j], elements[j + 1]);
        elements = document.querySelectorAll(".element");
      }

      elements[j].style.backgroundColor = "#0094FF";
      elements[j + 1].style.backgroundColor = "#0094FF";
    }

    elements[elements.length - i - 1].style.backgroundColor = "#00DD21";
  }
  elements[0].style.backgroundColor = "#00DD21";
};

generateNumbers();
