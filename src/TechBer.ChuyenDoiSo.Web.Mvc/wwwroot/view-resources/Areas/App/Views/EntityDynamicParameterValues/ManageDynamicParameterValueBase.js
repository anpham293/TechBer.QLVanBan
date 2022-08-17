var ManageDynamicParameterValueBase = (function ($) {
    return function () {
        var _entityDynamicParameterValueAppService = abp.services.app.entityDynamicParameterValue;
        var dataAndInputTypes = [];
        var _args;
        var _permissions = {
            delete: abp.auth.hasPermission('Pages.Administration.EntityDynamicParameterValue.Delete')
        };

        function initialize(args) {
            abp.ui.setBusy();
            _args = args;
            dataAndInputTypes = [];

            _entityDynamicParameterValueAppService
                .getAllEntityDynamicParameterValues({
                    entityFullName: args.entityFullName,
                    entityId: args.entityId
                })
                .done(function (data) {
                    var body = $(args.bodyElement);
                    if (!data || !data.items || data.items.length == 0) {
                        var row = $("<tr></tr>");

                        var td = $("<td class='text-center' colspan='3'></td>");
                        td.text(app.localize("ThereAreNoDynamicParametersMessage"));

                        row.append(td);
                        body.append(row);
                        return;
                    }

                    for (var i = 0; i < data.items.length; i++) {
                        var item = data.items[i];
                        var inputTypeManager = abp.inputTypeProviders.getInputTypeInstance({ inputType: item.inputType });

                        body.append(getRow(item, inputTypeManager));
                        inputTypeManager.afterViewInitialized();

                        dataAndInputTypes.push({
                            data: item,
                            inputTypeManager: inputTypeManager
                        });
                    }
                })
                .always(function () {
                    abp.ui.clearBusy();
                });
        }

        function getRow(item, inputTypeManager) {
            var view = inputTypeManager.getView(item.selectedValues, item.allValuesInputTypeHas);
            var row = $("<tr></tr>");

            var parameterNameTd = $("<td></td>");
            parameterNameTd.text(item.parameterName);

            var viewTd = $("<td></td>").append(view);
            viewTd.append(view);

            var actionsTd = $("<td></td>");
            if (_permissions.delete) {
                var btnDelete = $("<button class=\"btn btn-danger\"></button>")
                    .text(app.localize("Delete"))
                    .click(function () {
                        deleteAllValues({
                            dynamicParameterName: item.parameterName,
                            entityDynamicParameterId: item.entityDynamicParameterId
                        });
                    })
                actionsTd.append(btnDelete);
            }

            row.append(parameterNameTd);
            row.append(viewTd);
            row.append(actionsTd);

            return row;
        }

        function deleteAllValues(params) {
            abp.message.confirm(
                app.localize('DeleteEntityDynamicParameterValueMessage', params.dynamicParameterName),
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        abp.ui.setBusy();
                        _entityDynamicParameterValueAppService.cleanValues({
                            entityDynamicParameterId: params.entityDynamicParameterId,
                            entityId: _args.entityId
                        })
                            .done(function () {
                                abp.notify.success(
                                    app.localize('SuccessfullyDeleted')
                                );

                                if (typeof _args.onDeleteValues === 'function') {
                                    _args.onDeleteValues();
                                }
                            })
                            .always(function () {
                                abp.ui.clearBusy();
                            });
                    }
                }
            );
        }

        function save(onDoneCallback) {
            if (!dataAndInputTypes) {
                return;
            }

            abp.ui.setBusy();

            var newValues = [];
            for (var i = 0; i < dataAndInputTypes.length; i++) {
                newValues.push({
                    entityId: _args.entityId,
                    entityDynamicParameterId: dataAndInputTypes[i].data.entityDynamicParameterId,
                    values: dataAndInputTypes[i].inputTypeManager.getSelectedValues()
                });
            }

            _entityDynamicParameterValueAppService.insertOrUpdateAllValues({ Items: newValues })
                .done(function () {
                    abp.notify.success(app.localize("SavedSuccessfully"));
                    if (typeof onDoneCallback === 'function') {
                        onDoneCallback();
                    }

                }).always(function () {
                    abp.ui.clearBusy();
                });
        }

        return {
            initialize: initialize,
            save: save
        };
    };
})(jQuery);