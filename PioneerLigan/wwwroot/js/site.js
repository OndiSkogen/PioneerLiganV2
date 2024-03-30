$("#select-league").on("change", function () {
    var selectedId = $(this).val();

    window.location.href = window.location.href + '?selectedId=' + selectedId;
});

function Collapser(item) {

    var y = item.nextElementSibling.id;

    if ($("#" + y).hasClass("hideElement")) {

        $("#" + y).removeClass("hideElement");

    } else {

        $("#" + y).addClass("hideElement");

    }
}

const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]')
const tooltipList = [...tooltipTriggerList].map(tooltipTriggerEl => new bootstrap.Tooltip(tooltipTriggerEl))

$(document).ready(function () {
    
    var newDecks = [];
    var existingDecks = [];
    var addedDecks = [];

    function addDeck(deckName) {
        
        addedDecks.push(deckName);
        updateDecksList();

    }

    function removeDeck(deckName) {

        var index = addedDecks.indexOf(deckName);
        if (index !== -1) {
            addedDecks.splice(index, 1);
            updateDecksList();
        }

        var existingDeckIndex = existingDecks.indexOf(deckName);
        if (existingDeckIndex !== -1) {
            existingDecks.splice(existingDeckIndex, 1);
        }

        var newDeckIndex = newDecksindexOf(deckName);
        if (newDeckIndex !== -1) {
            newDecks.splice(newDeckIndex, 1);
        }
    }

    function updateDecksList() {

        var decksList = document.getElementById("decks-added-list");
        decksList.innerHTML = "";

        addedDecks.forEach(function (deckName) {
            var listItem = document.createElement("li");
            listItem.textContent = deckName;
            decksList.appendChild(listItem);

            var removeButton = document.createElement("button");
            removeButton.textContent = "Remove";
            removeButton.className = "btn btn-danger btn-sm";
            removeButton.onclick = function () {
                removeDeck(deckName);
            };
            listItem.appendChild(removeButton);
        });
    }

    $('#add-deck').click(function (e) {
        e.preventDefault(); // Prevent the default form submission

        console.log("add deck button clicked");
        var deck = {};

        // Check if a deck is selected from the existing decks dropdown
        var selectedDeckId = $('#deck-id').val();
        if (selectedDeckId !== "") {
            // Add the selected existing deck to the existing decks array
            existingDecks.push({
                id: selectedDeckId,
                name: $('#deck-id option:selected').text()
            });
            addDeck($('#deck-id option:selected').text());
        } else {
            // Collect data for a new deck
            deck.name = $('#deck-name').val();
            deck.superArchType = $('#deck-arch-typ').val();
            deck.colorAffiliation = $('#deck-color').val();

            // Add the new deck to the new decks array
            newDecks.push(deck);
            addDeck(deck.name);
        }

        // Clear the form fields for adding a new deck
        $('#deck-name').val("");
        $('#deck-arch-typ').val("");
        $('#deck-color').val("");

        console.log("new deck added: ", deck);
    });

    $('#done-btn').click(function (e) {
        e.preventDefault(); // Prevent the default form submission
        console.log("done button clicked");

        // Send both arrays in the same AJAX call
        var postData = {
            newDecks: newDecks,
            existingDecks: existingDecks,
            leagueEventId: $('#league-event-id').val()
        };

        var token = $('input[name="__RequestVerificationToken"]').val();
        console.log("Calling AJAX with token: ", token);

        $.ajax({
            url: '/MetaGame/Create?handler=UpdateData', // Adjust the URL as needed
            type: 'POST',
            headers: {
                RequestVerificationToken: token
            },
            contentType: 'application/json',
            async: true,
            data: JSON.stringify(postData),
            success: function (response) {
                // Handle success response
                console.log('success', response);
                window.location.href = '/';
            },
            error: function (xhr, status, error) {
                // Handle error
                console.log('error', error);
            }
        });

        console.log("Run complete");
    });
});
