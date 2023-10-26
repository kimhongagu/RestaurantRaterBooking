const colors = [
    {
        font: '#990f0f',
        background: '#ffbfbf'
    },
    {
        font: '#99630f',
        background: '#d6ffbf'
    },
    {
        font: '#6f7d4e',
        background: '#fff3bf'
    },
    {
        font: '#4e7d74',
        background: '#bff0ff'
    },
    {
        font: '#594e7d',
        background: '#c8bfff'
    },
    {
        font: '#7d4e76',
        background: '#ffbff0'
    }
]

const getRandomColor = () => {
    const randomIndex = Math.floor(Math.random() * colors.length);
    return colors[randomIndex];
}

const removeTag = (event) => {
    if (event.target.classList.contains('tag-close')) {
        event.target.parentElement.remove();
        updateHiddenTags();
    }
}

const updateHiddenTags = () => {
    const tagsContainer = document.querySelector('.tags-container');
    const hiddenTags = document.getElementById('hiddenTags');

    const tags = Array.from(tagsContainer.getElementsByClassName('tag-text'))
        .map(tagText => tagText.innerText);

    hiddenTags.value = tags.join(',');
};

const addTag = (event) => {
    // Sử dụng event.key thay vì event.keyCode vì event.keyCode đã lỗi thời
    if (event.key === 'Enter') {
        event.preventDefault(); // Ngăn chặn việc submit form

        const input = document.getElementById('input');
        const tagsContainer = document.querySelector('.tags-container');
        const color = getRandomColor();
        const value = event.target.value;

        if (value.trim() !== "") {
            const spanElement = document.createElement('span');

            spanElement.innerHTML = `
                <span class="tag-text">${value}</span>
                <span class="tag-close"> ⌫ </span>
            `;
            spanElement.classList.add('tag');
            spanElement.style.backgroundColor = color.background;
            spanElement.style.color = color.font;

            tagsContainer.appendChild(spanElement);
            input.value = '';
            updateHiddenTags();
        }
    }
};


window.onload = () => {
    const tagsContainer = document.querySelector('.tags-container');
    const input = document.getElementById('input');

    tagsContainer.addEventListener('click', removeTag);
    input.addEventListener('keypress', addTag);
};
