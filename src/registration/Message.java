/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package registration;

import java.util.Random;

/**
 *
 * @author RC_Student_lab
 */
public final class Message {
    private static int messageCount = 0;
    private String messageId;
    private int messageNumber;
    private String recipient;
    private String content;
    private String messageHash;

public Message(String recipient, String content, int messageNumber){
    
        this.messageId = generateMessageId();
        this.recipient = recipient;
        this.content = content;
        this.messageHash = createMessageHash(messageId, messageNumber, content);
        messageCount++;
}

private String generateMessageId() {
        Random rand = new Random();
        int id = 1000000000 + rand.nextInt(900000000);
        return String.valueOf(id);
}

public static boolean isValidMessageRecipient(String recipient){
    return recipient.matches("^\\+\\d{11,13}$");
}

public static boolean isValidMessageLength(String content){
    return content.length() <=250;
}

public String createMessageHash(String messageId,int messageNumber,String content) {
        String[] words = content.split("");
        String firstWord = words.length > 0 ? words[0] : "N/A";
        String lastWord = words.length > 1 ? words[words.length - 1] : "N/A";
        return (messageId.substring(0, 2) + ":" + messageNumber + ":" + firstWord + lastWord).toUpperCase();
}
 
public String getMessageId(){ return messageId;}
public int getMessageNumber(){return messageNumber;}
public String getRecipient(){ return recipient;}
public String getContent(){ return content;}
public String getMessageHash(){ return messageHash;}

}

