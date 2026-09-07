window.prefixForm = {
    submit: async (requestUri, name) => {
        const response = await fetch(requestUri, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ name }),
        });

        if (!response.ok) {
            throw new Error(`Form submission failed with status ${response.status}.`);
        }

        return response.json();
    },
};
