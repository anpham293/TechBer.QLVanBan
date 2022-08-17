(function () {
    $(function () {
        var _table = $('#SubscriptionTable');
        var _webhookSubscriptionService = abp.services.app.webhookSubscription;

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Administration.WebhookSubscription.Create'),
            edit: abp.auth.hasPermission('Pages.Administration.WebhookSubscription.Edit'),
            changeActivity: abp.auth.hasPermission('Pages.Administration.WebhookSubscription.ChangeActivity'),
            detail: abp.auth.hasPermission('Pages.Administration.WebhookSubscription.Detail')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/WebhookSubscription/CreateModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/WebhookSubscriptions/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditWebhookSubscriptionModal',
            cssClass: 'scrollable-modal'
        });

        var dataTable = _table.DataTable({
            paging: false,
            serverSide: false,
            processing: false,
            listAction: {
                ajaxFunction: _webhookSubscriptionService.getAllSubscriptions,
            },
            columnDefs: [
                {
                    className: 'control responsive',
                    orderable: false,
                    render: function () {
                        return '';
                    },
                    targets: 0
                },
                {
                    targets: 1,
                    data: "webhookUri",
                },
                {
                    targets: 2,
                    data: "webhooks",
                    render: function (webhooks) {
                        var result = "";
                        if (webhooks && webhooks.length > 0) {
                            for (var i = 0; i < webhooks.length; i++) {
                                if (i > 2) {
                                    result += ". . .";
                                    return result;
                                }
                                var webhook = webhooks[i];                             
                                result += webhook + "<br/>";
                            }
                        }
                        return result;
                    }
                },
                {
                    targets: 3,
                    data: "isActive",
                    render: function (isActive) {
                        var $span = $("<span/>").addClass("label");
                        if (isActive) {
                            $span.addClass("kt-badge kt-badge--success kt-badge--inline").text(app.localize('Yes'));
                        } else {
                            $span.addClass("kt-badge kt-badge--dark kt-badge--inline").text(app.localize('No'));
                        }

                        return $span[0].outerHTML;
                    }
                }
            ],
            drawCallback: function (settings) {
                if (_permissions.detail) {
                    _table.find('tbody tr').css("cursor", "pointer");
                }
            }
        });

        if (_permissions.detail) {
            _table.find('tbody').on('click', 'tr', function () {
                var data = dataTable.row(this).data();
                if(data){
                    window.location = "/App/WebhookSubscription/Detail/" + data.id;
                }
            });
        }

        $('#CreateNewWebhookSubscription').click(function () {
            _createOrEditModal.open();
        });

        $('#GetSubscriptionsButton').click(function (e) {
            e.preventDefault();
            getWebhooks();
        });

        function getWebhooks() {
            dataTable.ajax.reload();
        }

        abp.event.on('app.createOrEditWebhookSubscriptionModalSaved', function () {
            getWebhooks();
        });
    });
})();
