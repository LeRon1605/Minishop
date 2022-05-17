let deleteUser = (e) => {
    let id = e.dataset.id;
    let index = e.dataset.index;
    e.classList.add('disabled');
    axios({
        method: 'post',
        url: '/admin/user/delete',
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
        } else {
            e.classList.remove('disabled');
        }
        showToast(res.data.status, res.data.message);
    })
}
let resetPassword = (e) =>
{
    e.classList.add('disabled');
    axios({
        method: 'post',
        url: '/user/resetpassword',
        data: {
            email: e.dataset.email
        }
    }).then(res => {
        if (res.data.status) {
            showToast(res.data.status, "Đặt lại mật khẩu thành công");
        } else {
            showToast(res.data.status, "Đặt lại mật khẩu thất bại")
        }
        e.classList.remove('disabled');
    })
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