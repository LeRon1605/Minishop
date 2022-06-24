window.addEventListener('load', (e) => {
    const toast = document.getElementById('toast_body');
    const params = new Proxy(new URLSearchParams(window.location.search), {
        get: (searchParams, prop) => searchParams.get(prop),
    });
    $('#keyword_search').val(params.keyword || '');
    $('#value_search').val(params.Value || 'All');
    $('#state_search').val(params.State || 'All');
    if (toast.innerText.trim() != '') $("#notification_toast").toast('show');
});
$('#submitBtn').click(e => {
    let startDate = document.getElementById('startDate_voucher');
    let endDate = document.getElementById('endDate_voucher');
    let validationEndMessage = document.getElementById('validation-endDate-message');
    let validationStartMessage = document.getElementById('validation-startDate-message');
    const now = new Date();
    now.setHours(0, 0, 0, 0);
    if (new Date(startDate.value).getTime() < now.getTime()) {
        if (validationStartMessage.classList.contains('d-none')) validationStartMessage.classList.remove('d-none');
        e.preventDefault();
    } else {
        if (!validationStartMessage.classList.contains('d-none')) validationStartMessage.classList.add('d-none');
    }

    if (new Date(endDate.value).getTime() < new Date(startDate.value).getTime()) {
        if (validationEndMessage.classList.contains('d-none')) validationEndMessage.classList.remove('d-none');
        e.preventDefault();
    } else {
        if (!validationEndMessage.classList.contains('d-none')) validationEndMessage.classList.add('d-none');
    }

});

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
let deleteVoucher = (e) => {
    let id = e.dataset.id;
    let index = e.dataset.index;
    axios({
        method: 'post',
        url: '/admin/voucher/delete',
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

// Button action group
let action = document.getElementsByClassName('action_voucher');

// Content 
let listProduct = document.getElementById('list_voucher');
let addForm = document.getElementById('add_form');

for (let i = 0; i < action.length; i++) {
    action[i].addEventListener('click', (e) => {
        for (let j = 0; j < action.length; j++) {
            action[j].classList.toggle('action_voucher_click');
        }
        console.log(listProduct);
        listProduct.classList.toggle('d-none');
        addForm.classList.toggle('d-none');
    });
}
