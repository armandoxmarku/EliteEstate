// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function calculateMortgage() {
    var principal = document.getElementById("principal").value;
    var interestRate = document.getElementById("interestRate").value;
    var loanTerm = document.getElementById("loanTerm").value;

    var monthlyInterestRate = interestRate / 100 / 12;
    var numberOfPayments = loanTerm;
    var monthlyPayment = principal * (monthlyInterestRate * Math.pow(1 + monthlyInterestRate, numberOfPayments)) / (Math.pow(1 + monthlyInterestRate, numberOfPayments) - 1);

    document.getElementById("monthlyPayment").innerText = "Monthly Payment: " + "$" + monthlyPayment.toFixed(2);
}


document.addEventListener("DOMContentLoaded", function() {
    const propertyImages = document.querySelectorAll(".property-image");
    const thumbnails = document.querySelectorAll(".thumbnail");

    // Show the first image by default
    propertyImages[0].classList.add("active");

    thumbnails.forEach((thumbnail, index) => {
        thumbnail.addEventListener("click", function() {
            // Hide all property images
            propertyImages.forEach((image) => {
                image.classList.remove("active");
            });

            // Display the selected property image
            propertyImages[index].classList.add("active");

            // Update active state for thumbnails
            thumbnails.forEach((thumb) => {
                thumb.classList.remove("active");
            });
            thumbnail.classList.add("active");
        });
    });
});