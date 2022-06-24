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
            const voucherValue = document.getElementById('voucherValue');
            const totalValue = document.getElementById('totalValue');
            const total = document.getElementById('total');
            if (data.status) {
                notification.classList.add('alert-success');
                if (notification.classList.contains('alert-danger'))
                    notification.classList.remove('alert-danger');
                notification.innerHTML = data.message;
                $('#voucher').val($('#voucherSeri').val());
                voucherValue.innerText = '-' + Number(data.value.Value).toLocaleString() + ' đ';
                let money = Number(total.innerText.trim().replace(/[^0-9-]+/g, "")) - data.value.Value;
                totalValue.innerText = ((money < 0) ? '0' : money.toLocaleString()) + ' đ';
            } else {
                notification.classList.add('alert-danger');
                if (notification.classList.contains('alert-success'))
                    notification.classList.remove('alert-success');
                $('#voucher').val('');
                voucherValue.innerText = '-0 đ';
                notification.innerHTML = data.message;
                totalValue.innerText = (Number(total.innerText.trim().replace(/[^0-9-]+/g, ""))).toLocaleString() + ' đ';
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