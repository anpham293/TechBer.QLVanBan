(function ($) {
    app.modals.CreateOrEditDoiTuongChuyenDoiSoModal = function () {

        var _doiTuongChuyenDoiSosService = abp.services.app.doiTuongChuyenDoiSos;

        var _modalManager;
        var _$doiTuongChuyenDoiSoInformationForm = null;

        $("#DoiTuongChuyenDoiSo_Type").select2({
            data: [
                { id: 1, text: "PHU_LUC_IA" },
                { id: 2, text: "PHU_LUC_IIA" },
            ],
            width: '100%',
            minimumResultsForSearch: -1,
        });

        if ($("#DoiTuongChuyenDoiSoInformationsTab [name='id']").attr("value") !== undefined) {
            $('#DoiTuongChuyenDoiSo_Type').val($("#DoiTuongChuyenDoiSo_Type").attr("value")).trigger('change');
        }

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$doiTuongChuyenDoiSoInformationForm = _modalManager.getModal().find('form[name=DoiTuongChuyenDoiSoInformationsForm]');
            _$doiTuongChuyenDoiSoInformationForm.validate();
        };

        this.save = function () {
            if (!_$doiTuongChuyenDoiSoInformationForm.valid()) {
                return;
            }
            if ($('#DoiTuongChuyenDoiSo_UserId').prop('required') && $('#DoiTuongChuyenDoiSo_UserId').val() === '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('User')));
                return;
            }

            var doiTuongChuyenDoiSo = _$doiTuongChuyenDoiSoInformationForm.serializeFormToObject();
			
			_modalManager.setBusy(true);
			_doiTuongChuyenDoiSosService.createOrEdit(
                doiTuongChuyenDoiSo
			).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditDoiTuongChuyenDoiSoModalSaved');
			}).always(function () {
                _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);