const headers = {
    'api-key': process.env.BREVO_API_KEY,
    'content-type': 'application/json',
    accept: 'application/json',
};


export async function sendEmail(emailPayload) {
    emailPayload = {
        to: [
            {
                name: 'Beau HU',
                email: 'hu-beau@outlook.com',
            }
        ],
        templateId: 12,
        params: {
            name: 'Beau for Test',
        }
    }
    const response = await fetch('https://api.brevo.com/v3/smtp/email', {
        method: 'POST',
        headers,
        body: JSON.stringify({
            ...emailPayload,
            sender: {
                name: 'Idea Hunt',
                email: 'founder@idea-hunt.com',
            },
        }),
    });

    if (!response.ok) {
        logger.error('Failed to send email', response.statusText, payload);
        return false;
    }

    return true;
}



export async function sendEmailForIdeaHunt() {
    const emailPayload = {
        to: [
            {
                name: 'Beau HU',
                email: 'hu-beau@outlook.com',
            }
        ],
        templateId: 15,
        params: {
            customized_title: 'Better place to mention FeatBit', 
            // Zero cost to reach thousands of customers with your service.
            // 10x Revenue Growth via Reddit
            customized_message_based_on_company: "I discovered that FeatBit is an excellent open-source feature flag management tool that helps reduce release risks and improve product quality. I also noticed that FeatBit is experiencing rapid growth and is looking to attract more business customers. We would be delighted to help spread the word and let more people know about it.",
        }
    }
    const response = await fetch('https://api.brevo.com/v3/smtp/email', {
        method: 'POST',
        headers,
        body: JSON.stringify({
            ...emailPayload,
            sender: {
                name: 'Idea Hunt',
                email: 'founder@idea-hunt.com',
            },
        }),
    });

    if (!response.ok) {
        logger.error('Failed to send email', response.statusText, payload);
        return false;
    }

    return true;
}
