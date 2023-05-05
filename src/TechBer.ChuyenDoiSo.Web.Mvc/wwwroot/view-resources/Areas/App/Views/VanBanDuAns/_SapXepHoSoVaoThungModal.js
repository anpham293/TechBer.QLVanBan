(function ($) {
    app.modals.SapXepHoSoVaoThungModal = function () {

        var _vanBanDuAnsService = abp.services.app.vanBanDuAns;
        
        var _modalManager;
        var _$sapXepHoSoVaoThungInformationsForm = null;

        var _VanBanDuAnThungHoSoLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/VanBanDuAns/ThungHoSoLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/VanBanDuAns/_VanBanDuAnThungHoSoLookupTableModal.js',
            modalClass: 'ThungHoSoLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$sapXepHoSoVaoThungInformationsForm = _modalManager.getModal().find('form[name=SapXepHoSoVaoThungInformationsForm]');
            _$sapXepHoSoVaoThungInformationsForm.validate();
        };

        $('#OpenThungHoSoLookupTableButton').click(function () {

            var thungHoSo = _$sapXepHoSoVaoThungInformationsForm.serializeFormToObject();

            _VanBanDuAnThungHoSoLookupTableModal.open({id: thungHoSo.thungHoSoId, displayName: thungHoSo.thungHoSoName}, function (data) {
                _$sapXepHoSoVaoThungInformationsForm.find('input[name=thungHoSoName]').val(data.displayName);
                _$sapXepHoSoVaoThungInformationsForm.find('input[name=thungHoSoId]').val(data.id);
            });
        });

        $('#ClearThungHoSoNameButton').click(function () {
            _$sapXepHoSoVaoThungInformationsForm.find('input[name=thungHoSoName]').val('');
            _$sapXepHoSoVaoThungInformationsForm.find('input[name=thungHoSoId]').val('');
        });
        
        //Save
        this.save = function () {
            var sapXepHoSoVaoThung = _$sapXepHoSoVaoThungInformationsForm.serializeFormToObject();
            _modalManager.setBusy(true);
            _vanBanDuAnsService.sapXepHoSoVaoThung(
                sapXepHoSoVaoThung
            ).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditVanBanDuAnModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);