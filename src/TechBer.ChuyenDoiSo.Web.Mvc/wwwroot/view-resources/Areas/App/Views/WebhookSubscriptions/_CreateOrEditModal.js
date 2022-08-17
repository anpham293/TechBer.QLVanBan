(function () {
    app.modals.CreateOrEditWebhookSubscriptionModal = function () {
        var _modalManager;
        var _webhookSubscriptionService = abp.services.app.webhookSubscription;

        this.init = function (modalManager) {
            _modalManager = modalManager;
            _modalManager.getModal().find('#Webhooks').select2();
        };

        this.save = function () {
            var subscription = {
                id: _modalManager.getModal().find("input[name=Id]").val(),
                webhookUri: _modalManager.getModal().find("#webhookEndpointURL").val(),
                webhooks: _modalManager.getModal().find("#Webhooks").val(),
                headers: createOrEditHeaders
            };

            _modalManager.setBusy(true);

            if (createOrEditIsEdit) {
                _webhookSubscriptionService.updateSubscription(subscription)
                    .done(function () {
                        abp.notify.info(app.localize('SavedSuccessfully'));
                        _modalManager.close();
                        abp.event.trigger('app.createOrEditWebhookSubscriptionModalSaved');
                    }).always(function () {
                        _modalManager.setBusy(false);
                    });
            } else {
                _webhookSubscriptionService.addSubscription(subscription)
                    .done(function () {
                        abp.notify.info(app.localize('SavedSuccessfully'));
                        _modalManager.close();
                        abp.event.trigger('app.createOrEditWebhookSubscriptionModalSaved');
                    }).always(function () {
                        _modalManager.setBusy(false);
                    });
            }
        };

        $("#Btn_AddNewHeader").click(function () {
            AddNewHeader();
        });

        function AddNewHeader() {
            var headerKey = $("#CreateOrEditSubscription_HeaderKey").val();
            var headerValue = $("#CreateOrEditSubscription_HeaderValue").val();

            if (!headerKey || headerKey == '' || !headerValue || headerValue == '') {
                abp.notify.error(app.localize('HeaderKeyAndValueCanNotBeNull'));
                return;
            }

            if (createOrEditHeaders[headerKey]) {
                abp.notify.error(app.localize('HeaderKeysMustBeUnique'));
                return;
            }

            createOrEditHeaders[headerKey] = headerValue;
            addHeaderToListView(headerKey, headerValue);
        }

        function addHeaderToListView(key, value) {
            var row = `<div class="alert alert-elevate alert-light alert-bold m-1" id="additional-header-` + key + `" role="alert">
                        <div class="alert-text">[` + key + `, ` + value + `]</div>
                        <div class="alert-close">
                            <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                                <span aria-hidden="true"><i class="la la-close"></i></span>
                            </button>
                        </div>
                    </div>`;
            $("#additional-header-list").append(row);
        }

        $(document).on('click', '#additional-header-list .alert-close button', function () {
            var parent = $(this).closest(".alert");
            var key = parent.attr("id").replace("additional-header-", "");

            if (createOrEditHeaders[key]) {
                delete createOrEditHeaders[key];
            }
        });
    };
})();