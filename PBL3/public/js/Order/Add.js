let btnCheckVoucher = document.getElementById('btnCheckVoucher');
btnCheckVoucher.addEventListener('click', (e) => {
    e.target.classList.add('disabled');
    $.ajax({
        type: "POST",
        url: '/voucher/check',
        data: JSON.stringify({
            Seri: $('#voucherSeri').val()
        }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            const notification = document.getElementById('notification');
            if (data.status) {
                const voucherValue = document.getElementById('voucherValue');
                const totalValue = document.getElementById('totalValue');
                notification.classList.add('alert-success');
                notification.innerHTML = data.message;
                $('#voucher').val($('#voucherSeri').val());
                voucherValue.innerText = '-' + Number(data.value.Value).toLocaleString() + ' đ';
                totalValue.innerText = (Number(totalValue.innerText.trim().replace(/[^0-9-]+/g, "")) - data.value.Value).toLocaleString() + ' đ';
            } else {
                notification.classList.add('alert-danger');
                notification.innerHTML = data.message;
            }
            if (notification.classList.contains('d-none')) {
                notification.classList.remove('d-none');
                notification.classList.add('d-block');
            }
            e.target.classList.remove('disabled');
        },
    });
});
window.addEventListener('load', (e) => {
    const toast = document.getElementById('toast_body');
    if (toast.innerText.trim() != '') $("#notification_toast").toast('show');
})