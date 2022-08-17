var CurrentPage = function () {

    var handleLogin = function () {
        var $loginForm = $('.login-form');
        var $submitButton = $('#kt_login_signin_submit');

        $submitButton.click(function () {
            trySubmitForm();
        });

        $loginForm.validate({
            rules: {
                username: {
                    required: true
                },
                password: {
                    required: true
                }
            }
        });

        $loginForm.find('input').keypress(function (e) {
            if (e.which === 13) {
                trySubmitForm();
            }
        });

        $('a.social-login-icon').click(function () {
            var $a = $(this);
            var $form = $a.closest('form');
            $form.find('input[name=provider]').val($a.attr('data-provider'));
            $form.submit();
        });

        $loginForm.find('input[name=returnUrlHash]').val(location.hash);

        $('input[type=text]').first().focus();

        function trySubmitForm() {
            if (!$('.login-form').valid()) {
                return;
            }

            abp.ui.setBusy(
                null,
                abp.ajax({
                    contentType: app.consts.contentTypes.formUrlencoded,
                    url: $loginForm.attr('action'),
                    data: $loginForm.serialize(),
                    abpHandleError: false
                }).fail(function (error) {
                    if(abp.setting.getBoolean('App.UserManagement.UseCaptchaOnLogin') && typeof grecaptcha != "undefined"){
                        grecaptcha.reExecute();
                    }
                    abp.ajax.showError(error);
                })
            );
        }
    }

    return {
        init: function () {
            handleLogin();
        }
    };

}();