// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const form = document.getElementById("editForm");
const submitBtn = document.getElementById("submitBtn");
const imageInput = document.getElementById("Image");

    function getFormStateWithoutImage() {
        const formData = new FormData(form);

    // Supprimer le champ image de la comparaison
    formData.delete("Image");

    return JSON.stringify([...formData.entries()]);
    }

    const initialState = getFormStateWithoutImage();

    form.addEventListener("input", () => {
        const currentState = getFormStateWithoutImage();
    submitBtn.disabled = (currentState === initialState);
    });

if (imageInput != null) {
imageInput.addEventListener("change", () => {
    submitBtn.disabled = false;
});

}
