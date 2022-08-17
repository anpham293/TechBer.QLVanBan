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

        $('input[name=password]').focus();

        function trySubmitForm() {
            if (!$('.login-form').valid()) {
                return;
            }

            abp.ui.setBusy(
                null,
                abp.ajax({
                    contentType: app.consts.contentTypes.formUrlencoded,
                    url: $loginForm.attr('action'),
                    data: $loginForm.serialize()
                }).fail(function () {
                    if(abp.setting.getBoolean('App.UserManagement.UseCaptchaOnLogin') && typeof grecaptcha != "undefined"){
                        grecaptcha.reset();
                    }
                })
            );
        }

        function getLastUserInfo() {
            var userInfo = JSON.parse(abp.utils.getCookieValue('userInfo'));
            if (!userInfo) {
                window.location.replace(abp.appPath + "Account/Logout");
            }

            $('input[name=usernameOrEmailAddress]').val(userInfo.userName);
            $('#userName').text(userInfo.userName);

            $('#tenantName').text(userInfo.tenant ? userInfo.tenant : "Host");

            if (userInfo.profilePictureId) {
                abp.services.app.profile.getProfilePictureById(userInfo.profilePictureId)
                    .done(function (data) {
                        if (data.profilePicture) {
                            $("#profilePicture").attr("src", "data:image/png;base64, " + data.profilePicture);
                        } else {
                            $("#profilePicture").attr("src", "/Profile/GetDefaultProfilePicture");
                        }
                    }).fail(function () {
                        $("#profilePicture").attr("src", "/Profile/GetDefaultProfilePicture");
                    });
            } else {
                $("#profilePicture").attr("src", "/Profile/GetDefaultProfilePicture");
            }
        }

        getLastUserInfo();
    }

    return {
        init: function () {
            handleLogin();
        }
    };
}();