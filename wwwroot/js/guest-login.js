window.guestLogin = function (email, password) {
    document.querySelector('input[name="Input.Email"]').value = email;
    document.querySelector('input[name="Input.Password"]').value = password;

    document.querySelector('#loginButton').click();
}