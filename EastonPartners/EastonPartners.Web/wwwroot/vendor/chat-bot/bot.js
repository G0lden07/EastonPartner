document.addEventListener("DOMContentLoaded", function () {
    const chat = document.querySelector('#chat');
    const textBox = document.querySelector('#text-box');
    const waitingAnimation = document.getElementById('waiting-animation');
    let dotCount = 0;
    let dotInterval;

    function applyStylesToResponse(response) {
        const body = document.body;
        if (body.classList.contains("theme-light")) {
            response.style.backgroundColor = "gainsboro";
        } else if (body.classList.contains("theme-dark")) {
            response.style.backgroundColor = "#333333";
        }
        response.style.display = "table";
        response.style.padding = "10px";
        response.style.borderRadius = "10px";
    }

    function go() {
        var message = textBox.value;

        // Display the user's question on the screen
        const myText = document.createElement("p");
        myText.innerText = "Me: " + message;
        myText.style = "color: white; background-color: dodgerblue; display: table; padding: 10px; border-radius: 10px; margin-top: 10px;";
        chat.appendChild(myText);
    }

    // Apply styles to existing responses
    document.querySelectorAll(".response").forEach(applyStylesToResponse);

    // Observe the chat container for new responses
    const observer = new MutationObserver(function (mutations) {
        mutations.forEach(function (mutation) {
            mutation.addedNodes.forEach(function (node) {
                if (node.nodeType === Node.ELEMENT_NODE && node.classList.contains('response')) {
                    applyStylesToResponse(node);
                }
            });
        });
    });

    observer.observe(chat, {
        childList: true,
        subtree: true
    });

    window.go = go;

    // Animation for waiting message
    function animateDots() {
        if (waitingAnimation) {
            waitingAnimation.textContent = '.' + '.'.repeat(dotCount % 3);
            dotCount++;
        }
    }

    // Start the dot animation when the waiting indicator is shown
    const startDotAnimation = () => {
        textBox.value = "";

        dotCount = 0;
        dotInterval = setInterval(animateDots, 500);
    };

    // Stop the dot animation when the waiting indicator is hidden
    const stopDotAnimation = () => {
        clearInterval(dotInterval);
        if (waitingAnimation) {
            waitingAnimation.textContent = '.';
        }
    };

    // Use HTMX events to start and stop the animation
    document.body.addEventListener("htmx:configRequest", startDotAnimation);
    document.body.addEventListener("htmx:afterRequest", stopDotAnimation);
});
