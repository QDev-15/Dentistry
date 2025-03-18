$(document).ready(function () {
    let categoryCount = 0;

    $("#saveCategory").click(function () {
        let categoryName = $("#newCategoryName").val();
        let categoryType = $("#categoryType").val();
        let categoryPosition = $("#categoryPosition").val();
        let categoryAvatar = $("#categoryAvatar").val();
        let parentCategoryId = $("#parentCategory").val();

        if (categoryName.trim() !== "") {
            categoryCount++;
            let categoryId = `category${categoryCount}`;
            let newCategory = createCategoryElement(categoryId, categoryName, categoryType, categoryPosition, categoryAvatar);

            if (parentCategoryId) {
                let parentCollapse = $("#collapse" + parentCategoryId + " .nested-accordion");
                if (!parentCollapse.length) {
                    let parentBody = $("#collapse" + parentCategoryId + " .accordion-body");
                    parentBody.append('<div class="accordion nested-accordion"></div>');
                    parentCollapse = parentBody.find(".nested-accordion");
                }
                parentCollapse.append(newCategory);
            } else {
                $("#categoryAccordion").append(newCategory);
            }

            $("#parentCategory").append(`<option value="${categoryId}">${categoryName}</option>`);
            $("#addCategoryModal").modal("hide");
            $("#newCategoryName, #categoryType, #categoryPosition, #categoryAvatar").val("");
        }
    });

    function createCategoryElement(id, name, type, position, avatar) {
        return `<div class="accordion-item">
                    <h2 class="accordion-header" id="heading${id}">
                        <button class="accordion-button" type="button" data-bs-toggle="collapse" data-bs-target="#collapse${id}" aria-expanded="true" aria-controls="collapse${id}">
                            <div class="category-item">
                                <img src="${avatar}" class="category-avatar" alt="Avatar">
                                <span>${name} (Vị trí: ${position}, Loại: ${type})</span>
                            </div>
                        </button>
                    </h2>
                    <div id="collapse${id}" class="accordion-collapse collapse" aria-labelledby="heading${id}">
                        <div class="accordion-body d-flex justify-content-between">
                            <span>${name}</span>
                            <div>
                                <button class="btn btn-warning btn-sm edit-btn">Sửa</button>
                                <button class="btn btn-danger btn-sm delete-btn">Xóa</button>
                            </div>
                        </div>
                    </div>
                </div>`;
    }

    $(document).on("click", ".delete-btn", function () {
        $(this).closest(".accordion-item").remove();
        let optionValue = $(this).closest(".accordion-item").attr("id");
        $("#parentCategory option[value='" + optionValue + "']").remove();
    });
});