export default
  {
    iconLinks: [
      {
        icon: 'github',
        href: 'https://github.com/Macad3D/Macad3D',
        title: 'GitHub'
      }
    ],
    showLightbox: () => false
  }

function handleAPngClick(event) {
  const target = event.currentTarget;
  if (target.classList.contains("img-swap")) {
    if (target.src.includes(".png")) {
      target.src = target.src.replace(".png", ".apng");
    } else {
      target.src = target.src.replace(".apng", ".png");
    }
    target.classList.toggle("on");
  }
}

async function init() {
  document.querySelectorAll(`.img-swap`).forEach(element => {
    element.addEventListener('click', handleAPngClick);
  });
}

init();