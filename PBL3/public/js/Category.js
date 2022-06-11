let action = document.getElementsByClassName('action_product');

// Content 
let listCategory = document.getElementById('list_product');
let addForm = document.getElementById('add_form');

for (let i = 0; i < action.length; i++) {
    action[i].addEventListener('click', (e) => {
        for (let j = 0; j < action.length; j++) {
            action[j].classList.toggle('action_product_click');
        }
        listCategory.classList.toggle('d-none');
        addForm.classList.toggle('d-none');
    });
}
let showToast = (status, message) => {
    const toastBody = document.getElementById('toast_body');
    if (status) {
        if (toastBody.classList.contains('bg-danger')) toastBody.classList.remove('bg-danger');
        if (!toastBody.classList.contains('bg-success')) toastBody.classList.add('bg-success');
    } else {
        if (!toastBody.classList.contains('bg-danger')) toastBody.classList.add('bg-danger');
        if (toastBody.classList.contains('bg-success')) toastBody.classList.remove('bg-success');
    }
    toastBody.innerText = message;
    $("#notification_toast").toast('show');
}
let deleteCategory = (e) => {
    let id = e.dataset.id;
    let index = e.dataset.index;
    axios({
        method: 'post',
        url: '/admin/category/delete',
        data: {
            id
        }
    }).then(res => {
        if (res.data.status) {
            let STT = document.getElementsByClassName('STT');
            for (let i = index; i < STT.length; i++) {
                STT[i].innerText = parseInt(STT[i].innerText) - 1;
                STT[i].dataset.index = parseInt(STT[i].dataset.index) - 1;
            };
            e.parentElement.parentElement.parentElement.remove();
        }
        showToast(res.data.status, res.data.message);
    })
}