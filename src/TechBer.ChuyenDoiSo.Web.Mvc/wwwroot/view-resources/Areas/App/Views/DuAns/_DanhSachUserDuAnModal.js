(function ($) {
    app.modals.DanhSachUserDuAn = function () {
        
        var _duAnsService = abp.services.app.duAns;
        
        var _addUserDuAn = new app.ModalManager({
            viewUrl: abp.appPath + 'App/DuAns/AddUserDuAnModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/DuAns/_AddUserDuAnModal.js',
            modalClass: 'AddUserDuAn',
            addMemberOptions: {
                title: app.localize('SelectAUser'),
                serviceMethod: _duAnsService.findUsers
            }
        });

        $('#AddUserDuAnButton').click(function () {
            _addUserDuAn.open();
        });
    };
})(jQuery);