﻿(function () {
    $(function () {

        var _$vanBanDuAnsTable = $('#VanBanDuAnsTable');
        var _vanBanDuAnsService = abp.services.app.vanBanDuAns;
        var _entityTypeFullName = 'TechBer.ChuyenDoiSo.QLVB.VanBanDuAn';
        var _$quyTrinhDuAnsTable = $('#QuyTrinhDuAnsTable');
        
        var _duAnSelectObject = $("#DuAnNameFilterId");
        var _quyTrinhDuAnsService = abp.services.app.quyTrinhDuAnAssigneds;
        var trees=$("#tree");
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.VanBanDuAns.Create'),
            edit: abp.auth.hasPermission('Pages.VanBanDuAns.Edit'),
            'delete': abp.auth.hasPermission('Pages.VanBanDuAns.Delete'),
            createQuyTrinhDuAn: abp.auth.hasPermission('Pages.QuyTrinhDuAnAssigneds.Create'),
            editQuyTrinhDuAn: abp.auth.hasPermission('Pages.QuyTrinhDuAnAssigneds.Edit'),
            deleteQuyTrinhDuAn: abp.auth.hasPermission('Pages.QuyTrinhDuAnAssigneds.Delete'),
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/VanBanDuAns/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/VanBanDuAns/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditVanBanDuAnModal'
        });
        var _createOrEditQuyTrinhDuAnAssignedsModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/QuyTrinhDuAnAssigneds/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/QuyTrinhDuAnAssigneds/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditQuyTrinhDuAnAssignedModal'
        });

        var _viewVanBanDuAnModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/VanBanDuAns/ViewvanBanDuAnModal',
            modalClass: 'ViewVanBanDuAnModal',
            modalSize: "modal-xl"
        });

        var _entityTypeHistoryModal = app.modals.EntityTypeHistoryModal.create();

        function entityHistoryIsEnabled() {
            return abp.auth.hasPermission('Pages.Administration.AuditLogs') &&
                abp.custom.EntityHistory &&
                abp.custom.EntityHistory.IsEnabled &&
                _.filter(abp.custom.EntityHistory.EnabledEntities, entityType => entityType === _entityTypeFullName).length === 1;
        }

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z");
        }

        var dataTable = _$vanBanDuAnsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _vanBanDuAnsService.getAll,
                inputFilter: function () {
                    return {
                        filter: $('#VanBanDuAnsTableFilter').val(),
                        nameFilter: $('#NameFilterId').val(),
                        kyHieuVanBanFilter: $('#KyHieuVanBanFilterId').val(),
                        minNgayBanHanhFilter: getDateFilter($('#MinNgayBanHanhFilterId')),
                        maxNgayBanHanhFilter: getDateFilter($('#MaxNgayBanHanhFilterId')),
                        fileVanBanFilter: $('#FileVanBanFilterId').val(),
                        duAnNameFilter: $('#DuAnNameFilterId').val(),
                        quyTrinhDuAnNameFilter: $('#QuyTrinhDuAnNameFilterId').val()
                    };
                }
            },
            columnDefs: [
                {
                    width: 120,
                    targets: 0,
                    data: null,
                    orderable: false,
                    fixedColumns: true,
                    defaultContent: '',
                    rowAction: {
                        cssClass: 'btn btn-brand dropdown-toggle',
                        text: '<i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span>',
                        items: [
                            {
                                text: app.localize('View'),
                                action: function (data) {
                                    _viewVanBanDuAnModal.open({id: data.record.vanBanDuAn.id});
                                }
                            },
                            {
                                text: app.localize('Edit'),
                                visible: function () {
                                    return _permissions.edit;
                                },
                                action: function (data) {
                                    _createOrEditModal.open({id: data.record.vanBanDuAn.id});
                                }
                            },
                            {
                                text: app.localize('History'),
                                visible: function () {
                                    return entityHistoryIsEnabled();
                                },
                                action: function (data) {
                                    _entityTypeHistoryModal.open({
                                        entityTypeFullName: _entityTypeFullName,
                                        entityId: data.record.vanBanDuAn.id
                                    });
                                }
                            },
                            {
                                text: app.localize('Delete'),
                                visible: function () {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    deleteVanBanDuAn(data.record.vanBanDuAn);
                                }
                            }]
                    }
                },
                {
                    width: '250px',
                    targets: 1,
                    data: "vanBanDuAn.name",
                    name: "name",
                    render: function (name) {
                        return '<a style="white-space: normal">'+name+'</a>';
                    }
                },
                {
                    targets: 2,
                    data: "vanBanDuAn.kyHieuVanBan",
                    name: "kyHieuVanBan"
                },
                {
                    targets: 3,
                    data: "vanBanDuAn.ngayBanHanh",
                    name: "ngayBanHanh",
                    render: function (ngayBanHanh) {
                        if (ngayBanHanh) {
                            return moment(ngayBanHanh).format('L');
                        }
                        return "";
                    }

                },
                {
                    targets: 4,
                    data: "vanBanDuAn.fileVanBan",
                    name: "fileVanBan",
                    render: function (displayName, type, row, meta) {
                        return "<a class='text-info text-bold link-view' data-target='" + row.vanBanDuAn.id + "' style='cursor: pointer;color:whitesmoke'>" + row.vanBanDuAn.fileVanBan + "</a>";
                    }
                },
                {
                    targets: 5,
                    data: "duAnName",
                    name: "duAnFk.name"
                },
                {
                    targets: 6,
                    data: "quyTrinhDuAnName",
                    name: "quyTrinhDuAnFk.name"
                },
                {
                    targets: 7,
                    data: "vanBanDuAn.viTriLuuTru",
                    name: "viTriLuuTru",
                }
            ],
            // initComplete
        });

        function getVanBanDuAns() {
            dataTable.ajax.reload();
        }

        function deleteVanBanDuAn(vanBanDuAn) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _vanBanDuAnsService.delete({
                            id: vanBanDuAn.id
                        }).done(function () {
                            getVanBanDuAns(true);
                            abp.notify.success(app.localize('SuccessfullyDeleted'));
                        });
                    }
                }
            );
        }

        $('#ShowAdvancedFiltersSpan').click(function () {
            $('#ShowAdvancedFiltersSpan').hide();
            $('#HideAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideDown();
        });

        $('#HideAdvancedFiltersSpan').click(function () {
            $('#HideAdvancedFiltersSpan').hide();
            $('#ShowAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideUp();
        });

        $('#CreateNewVanBanDuAnButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportToExcelButton').click(function () {
            _vanBanDuAnsService
                .getVanBanDuAnsToExcel({
                    filter: $('#VanBanDuAnsTableFilter').val(),
                    nameFilter: $('#NameFilterId').val(),
                    kyHieuVanBanFilter: $('#KyHieuVanBanFilterId').val(),
                    minNgayBanHanhFilter: getDateFilter($('#MinNgayBanHanhFilterId')),
                    maxNgayBanHanhFilter: getDateFilter($('#MaxNgayBanHanhFilterId')),
                    fileVanBanFilter: $('#FileVanBanFilterId').val(),
                    duAnNameFilter: $('#DuAnNameFilterId').val(),
                    quyTrinhDuAnNameFilter: $('#QuyTrinhDuAnNameFilterId').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditVanBanDuAnModalSaved', function () {
            getVanBanDuAns();
        });

        $('#GetVanBanDuAnsButton').click(function (e) {
            e.preventDefault();
            getVanBanDuAns();
        });
        class Cay {
            XOA_STATE = {
                XOA_THANH_CONG: 1,
                CON_NODE_CON: 2,
                CO_LOI: 3
            }

            DI_CHUYEN_STATE = {
                THANH_CONG: 1,
                THAT_BAI: 2
            }

            constructor(id, rootBtn) {
                this.eleId = id;
                this.$tree = $('#' + id);
                this.loaiDuAn = 0;
                this.show = function () {
                    this.$tree.show();
                };
                this.hide = function () {
                    this.$tree.hide();
                };
                this.unitCount = 0;
                this.$rootBtn = $('#' + rootBtn);
            }

            setUnitCount(unitCount) {
                this.unitCount = unitCount;
                if (unitCount) {
                    this.show();
                } else {
                    this.hide();
                }
            }

            selectedOu = {
                id: null,
                displayName: null,
                code: null,

                set(ouInTree) {
                    if (!ouInTree) {
                        this.selectedOu.id = null;
                        this.selectedOu.displayName = null;
                        this.selectedOu.code = null;
                    } else {
                        this.selectedOu.id = ouInTree.id;
                        this.selectedOu.displayName = ouInTree.original.displayName;
                        this.selectedOu.code = ouInTree.original.code;
                    }

                }
            }

            refreshUnitCount() {
                this.setUnitCount(this.$tree.jstree('get_json').length);
            }

            addUnit(parentId) {
                let self = this

                var instance = $.jstree.reference(self.$tree);

                _createOrEditQuyTrinhDuAnAssignedsModal.open({
                    parentId: parentId,
                    loaiDuAn: self.loaiDuAn
                }, function (newOu) {
                    instance.create_node(
                        parentId ? instance.get_node(parentId) : '#',
                        {
                            id: newOu.quyTrinhDuAnAssigned.id,
                            parent: newOu.quyTrinhDuAnAssigned.parentId ? newOu.quyTrinhDuAnAssigned.parentId : '#',
                            code: newOu.quyTrinhDuAnAssigned.maQuyTrinh,
                            displayName: newOu.quyTrinhDuAnAssigned.displayName,
                            memberCount: 0,
                            roleCount: 0,
                            text: self.generateTextOnTree(newOu.quyTrinhDuAnAssigned),
                            state: {
                                opened: true
                            }
                        });
                    self.refreshUnitCount();
                    self.reload();
                });
            }

            generateTextOnTree(ou) {
                var itemClass = ' ou-text-has-members';

                return '<span class="ou-text text-dark tooltipss' + itemClass + '" data-ou-id="' + ou.id + '"><b>' + ou.maQuyTrinh + '</b> ' +
                    app.htmlUtils.htmlEncodeText(ou.name) +
                    ' <i class="fa fa-caret-down text-muted"></i> ' +
                    ' <span style="font-size: .82em; opacity: .5;">' +
                    '</span>'+((ou.descriptions!=="")?'<div class="tooltipsstext">'+ou.descriptions+'</div>':"")+'</span>';
            }

            incrementMemberCount(ouId, incrementAmount) {
                var treeNode = this.$tree.jstree('get_node', ouId);
                treeNode.original.memberCount = treeNode.original.memberCount + incrementAmount;
                this.$tree.jstree('rename_node', treeNode, this.generateTextOnTree(treeNode.original));
            }

            incrementRoleCount(ouId, incrementAmount) {
                var treeNode = this.$tree.jstree('get_node', ouId);
                treeNode.original.roleCount = treeNode.original.roleCount + incrementAmount;
                this.$tree.jstree('rename_node', treeNode, this.generateTextOnTree(treeNode.original));
            }

            getTreeDataFromServer(loai_id,callback) {
                let self = this;
                _quyTrinhDuAnsService.getDataForTree(loai_id).done(function (result) {
                    var treeData = _.map(result, function (item) {

                        return {
                            id: item.quyTrinhDuAn.id,
                            parent: item.quyTrinhDuAn.parentId ? item.quyTrinhDuAn.parentId : '#',
                            code: '0001',
                            displayName: item.quyTrinhDuAn.stt + ' ' + item.quyTrinhDuAn.name,
                            memberCount: 0,
                            roleCount: 0,
                            text: self.generateTextOnTree(item.quyTrinhDuAn),
                            sapXep: item.quyTrinhDuAn.stt ?? 0,
                            state: {
                                // opened: true
                            }
                        };
                    });

                    callback(treeData);
                });
            }

            init(loai_id) {
                let self = this;
                abp.ui.setBusy('#' + self.eleId)

                this.loaiDuAn = loai_id;


                this.getTreeDataFromServer(loai_id,function (treeData) {

                    self.setUnitCount(treeData.length)
                    self.$tree.on('changed.jstree', function (e, data) {
                    })
                        .on('move_node.jstree', function (e, data) {

                            console.log(data);

                            var parentNodeName = (!data.parent || data.parent === '#')
                                ? app.localize('Root')
                                : self.$tree.jstree('get_node', data.parent).original.displayName;

                            abp.message.confirm(
                                app.localize('XacNhanDiChuyenTieuChi', data.node.original.displayName, parentNodeName),
                                app.localize('AreYouSure'),
                                function (isConfirmed) {
                                    if (isConfirmed) {
                                        _quyTrinhDuAnsService.moveTieuChi({
                                            id: data.node.id,
                                            position: data.position,
                                            newParentId: data.parent === '#' ? null : data.parent
                                        }).done(function (ketQua) {
                                            if (ketQua == self.DI_CHUYEN_STATE.THANH_CONG) {
                                                abp.notify.success(app.localize('SuccessfullyMoved'));
                                                self.reload();
                                            } else {
                                                self.$tree.jstree('refresh'); //rollback
                                                setTimeout(function () {
                                                    abp.message.error("Di chuyển thất bại");
                                                }, 500);
                                            }
                                        }).fail(function (err) {
                                            self.$tree.jstree('refresh'); //rollback
                                            setTimeout(function () {
                                                abp.message.error(err.message);
                                            }, 500);
                                        });
                                    } else {
                                        self.$tree.jstree('refresh'); //rollback
                                    }
                                }
                            );
                        })
                        .jstree({
                            'core': {
                                data: treeData,
                                multiple: false,
                                check_callback: function (operation, node, node_parent, node_position, more) {
                                    abp.ui.clearBusy();
                                    return true;
                                }
                            },
                            types: {
                                "default": {
                                    "icon": "fa fa-folder kt--font-warning"
                                },
                                "file": {
                                    "icon": "fa fa-file  kt--font-warning"
                                }
                            },
                            contextmenu: {
                                items: function (node) {
                                    var items = {
                                        editUnit: {
                                            label: app.localize('Edit'),
                                            icon: 'la la-pencil',
                                            _disabled: !_permissions.edit,
                                            action: function (data) {
                                                var instance = $.jstree.reference(data.reference);
                                                console.log(node);
                                                _createOrEditQuyTrinhDuAnAssignedsModal.open({id: node.id, parentId: (node.parent==="#")?null:node.parent,
                                                    loaiDuAn: self.loaiDuAn}, function (updatedOu) {
                                                    self.reload();
                                                    //console.log(node)
                                                    //node.original.sapXep = updatedOu.displayName;
                                                    //instance.rename_node(node, self.generateTextOnTree(updatedOu.quyTrinhDuAn));
                                                });
                                            }
                                        },

                                        addSubUnit: {
                                            label: 'Thêm quy trình con',
                                            icon: 'la la-plus',
                                            _disabled: !_permissions.create,
                                            action: function () {
                                                self.addUnit(node.id);
                                            }
                                        },
                                        addVanBanUnit: {
                                            label: 'Thêm văn bản',
                                            icon: 'la la-plus',
                                            _disabled: !_permissions.create,
                                            action: function () {
                                                _createOrEditModal.open({
                                                    duanid: $('#DuAnNameFilterId').val(),
                                                    quytrinhid: node.id
                                                });
                                            }
                                        },

                                        'delete': {
                                            label: app.localize("Delete"),
                                            icon: 'la la-remove',
                                            _disabled: !_permissions.delete,
                                            action: function (data) {
                                                var instance = $.jstree.reference(data.reference);

                                                abp.message.confirm(
                                                    app.localize('XoaTieuChi', node.original.displayName),
                                                    app.localize('AreYouSure'),
                                                    function (isConfirmed) {
                                                        if (isConfirmed) {
                                                            _quyTrinhDuAnsService.xoaTieuChi(node.id).done(function (result) {
                                                                if (result == self.XOA_STATE.XOA_THANH_CONG) {
                                                                    abp.notify.success(app.localize('SuccessfullyDeleted'));
                                                                    instance.delete_node(node);
                                                                    self.refreshUnitCount();
                                                                } else if (result == self.XOA_STATE.CON_NODE_CON) {
                                                                    abp.message.warn(
                                                                        'Không thể xóa khi vẫn còn tiêu chí con',
                                                                        'Lưu ý'
                                                                    )
                                                                } else {
                                                                    abp.message.warn(
                                                                        'Có lỗi xảy ra',
                                                                        'Lưu ý'
                                                                    )
                                                                }

                                                            }).fail(function (err) {
                                                                setTimeout(function () {
                                                                    abp.message.error(err.message);
                                                                }, 500);
                                                            });
                                                        }
                                                    }
                                                );
                                            }
                                        }
                                    };

                                    if (entityHistoryIsEnabled()) {
                                        items.history = {
                                            label: app.localize('History'),
                                            icon: 'la la-history',
                                            _disabled: !_permissions.cayTieuChi,
                                            action: function () {
                                                _entityTypeHistoryModal.open({
                                                    entityTypeFullName: _entityTypeFullName,
                                                    entityId: node.original.id,
                                                    entityTypeDescription: node.original.displayName
                                                });
                                            }
                                        };
                                    }

                                    return items;
                                }
                            },
                            sort: function (node1, node2) {
                                if (parseInt(this.get_node(node2).original.sapXep) < parseInt(this.get_node(node1).original.sapXep)) {
                                    return 1;
                                }
                                return -1;
                            },
                            plugins: [
                                'types',
                                'contextmenu',
                                'wholerow',
                                'sort',
                                'state',
                                'dnd'
                            ]
                        });
                    self.$rootBtn.click(function (e) {
                        self.$rootBtn.off("click");
                        e.preventDefault();
                        self.addUnit();
                        self.$rootBtn.off("click");
                    });

                    self.$tree.on("select_node.jstree", function (e, data) {
                        var elementId = self.$tree.jstree("get_selected",true)[0]["a_attr"]['id'];
                        var value = data.node.id;
                        $(".text-danger").removeClass("text-danger").addClass("text-info");
                        $('#QuyTrinhDuAnNameFilterId').val(value);
                        $("#"+elementId).removeClass("text-info").addClass("text-danger");
                        getVanBanDuAns();
                    });
                    self.$tree.on('click', '.ou-text .fa-caret-down', function (e) {
                        e.preventDefault();

                        var ouId = $(this).closest('.ou-text').attr('data-ou-id');
                        setTimeout(function () {
                            self.$tree.jstree('show_contextmenu', ouId);
                        }, 100);
                    });
                    self.$tree.on('click', '.jstree-icon.jstree-ocl', function (e) {
                        e.preventDefault();
                        self.$tree.jstree('save_state');
                    });
                    $("#Collapse").on("click",function (){
                        var bt = $(this);
                        self.$tree.jstree('close_all');
                        $("#Expand").removeAttr("hidden");
                        bt.attr("hidden","hidden");
                        self.$tree.jstree('save_state');
                    });
                    $("#Expand").on("click",function (){
                        var bt = $(this);
                        self.$tree.jstree('open_all');
                        $("#Collapse").removeAttr("hidden");
                        bt.attr("hidden","hidden");
                        self.$tree.jstree('save_state');
                    });
                    $("#reload-tree").on("click",function (){
                        self.$tree.jstree('save_state');
                        self.reload();
                    });
                });
            }

            reload() {
                let self = this;
                self.getTreeDataFromServer(_duAnSelectObject.val(),function (treeData) {
                    
                    self.setUnitCount(treeData.length);
                    self.$tree.jstree(true).settings.core.data = treeData;
                    self.$tree.jstree('refresh');
                });
            }
        }
        var CayPhuLucIA = new Cay('tree', 'AddRootUnitButtonIA');
        CayPhuLucIA.init(_duAnSelectObject.val());
        function getQuyTrinhDuAns() {
            dataTablequytrinh.ajax.reload();
        }

        function deleteQuyTrinhDuAn(quyTrinhDuAn) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _quyTrinhDuAnsService.delete({
                            id: quyTrinhDuAn.id
                        }).done(function () {
                            getQuyTrinhDuAns(true);
                            abp.notify.success(app.localize('SuccessfullyDeleted'));
                        });
                    }
                }
            );
        }

        $('#ShowAdvancedFiltersSpan').click(function () {
            $('#ShowAdvancedFiltersSpan').hide();
            $('#HideAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideDown();
        });

        $('#HideAdvancedFiltersSpan').click(function () {
            $('#HideAdvancedFiltersSpan').hide();
            $('#ShowAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideUp();
        });

        $("#CreateVanBan").on("click", function () {
            var self = $('#QuyTrinhDuAnNameFilterId');
            if(self.val()===""){
                abp.message.warn("Chưa chọn quy trình cần thêm văn bản!");
            }
            else{
                _createOrEditModal.open({
                    duanid: $('#DuAnNameFilterId').val(),
                    quytrinhid: self.val()
                });
            }
        });
        _duAnSelectObject.on("change",function () {
            var self=$(this);
            var d = trees.parent();
            trees=$(document).find("#tree");
            trees.remove();
            loai_id=self.attr("data-target");
            d.append("<div id='tree'></div>");
            var CayPhuLucIA = new Cay('tree', 'AddRootUnitButtonIA');
            CayPhuLucIA.init(self.val());
        })
        $('#ExportToExcelButton').click(function () {
            _quyTrinhDuAnsService
                .getQuyTrinhDuAnsToExcel({
                    filter: $('#QuyTrinhDuAnsTableFilter').val(),
                    nameFilter: $('#NameFilterId').val(),
                    descriptionsFilter: $('#DescriptionsFilterId').val(),
                    loaiDuAnNameFilter: $('#LoaiDuAnNameFilterId').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditQuyTrinhDuAnModalSaved', function () {
            getQuyTrinhDuAns();
        });

        $('#GetQuyTrinhDuAnsButton').click(function (e) {
            e.preventDefault();
            getQuyTrinhDuAns();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getVanBanDuAns();

            }
        });
        $(document).on("click", ".btn-reload-vanban", function () {
            var self = $(this);
            $(".text-danger").removeClass("text-danger").addClass("text-info");
            $('#QuyTrinhDuAnNameFilterId').val(self.attr("data-target"));
            self.removeClass("text-info").addClass("text-danger");
            getVanBanDuAns();
        });
        $(document).on("click", ".link-view", function () {
            var self = $(this);
            _viewVanBanDuAnModal.open({id: self.attr("data-target")});
        });
        $('#filall').click(function (e) {
            e.preventDefault();
            $('#QuyTrinhDuAnNameFilterId').val("");
            getVanBanDuAns();
            $(".text-danger").removeClass("text-danger").addClass("text-info");
        });
    });
})();