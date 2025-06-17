/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package registration;

import java.io.FileWriter;
import java.util.ArrayList;
import java.util.Collections;
import java.util.Comparator;
import java.util.Iterator;
import javax.swing.JOptionPane;
import org.json.simple.JSONObject;



/**
 *
 * @author RC_Student_lab
 */
public class ChatApp {
    private ArrayList<Message> sentMessages = new ArrayList<>();
    private ArrayList<Message> disregardedMessages = new ArrayList<>();
    private ArrayList<Message> storedMessages = new ArrayList<>();
    private int totalSent = 0;
    private int messageCount = 0;
    
    public ChatApp(){
        String[][] testData ={
            {"+27834537896", "Did you get the cake?","Sent"},
            {"+27838884567","Where are you?You are late! I have asked you to be on time.","Stored"},
            {"+27834484567","Yohoo, I am at your gate.","Disregard"},
            {"0838884567","It is dinner time!","Sent"},
            {"+27838894567","Ok, I am leaving without you.","Stored"},
        };
        
        for (String[] data: testData) {
            String recipient = data[0];
            String message = data[1];
            String flag = data[2];
            
            Message msg = new Message(recipient,message,++messageCount);
            
            if(flag.equals("Sent")){
                sentMessages.add(msg);
            }else if(flag.equals("Stored")){
                storedMessages.add(msg);
            }else if(flag.equals("Disregard")){
                disregardedMessages.add(msg);
            }
        }
    }
    
    /**
     *
     */
   
    
    public void start() {
      JOptionPane.showMessageDialog(null, "Welcome to ChatApp");  
                  
         while (true){
             String option = JOptionPane.showInputDialog("""
                                                         Menu:
                                                         1.Send Message
                                                         2. View All Sent Messages
                                                         3. View Longest Message
                                                         4. Search Message by ID
                                                         5. Search Messages by Recipient
                                                         6. Delete Message by Hash
                                                         7. Show Message Report
                                                         8.Quit
                                                         """);
             if (option == null || option.equals("8")) {
                JOptionPane.showMessageDialog(null, "Exiting QuickChat. Goodbye!");
                break;
             }

            switch (option) {
                case "1":
                    sendMessage();
                    break;
                case "2":
                    showAllSentMessages();
                    break;
                case "3":
                    displayLongestMessage();
                    break;
                case "4":
                    searchByID();
                    break;
                case "5":
                    searchByRecipient();
                    break;
                case "6":
                    deleteByHash();
                    break;
                case "7":
                    displayReport();
                    break;
                default:
                    JOptionPane.showMessageDialog(null, "Invalid option. Try again.");
            }
        }
    }

    private void sendMessage() {
        
         int count = Integer.parseInt(JOptionPane.showInputDialog("How many messages do you want to send?"));
         for (int i=0; i < count; i++){
        String recipient = JOptionPane.showInputDialog("Enter recipient cell (+27...):");
        if (!Message.isValidMessageRecipient(recipient)){
            JOptionPane.showMessageDialog(null, "Invalid recipient number.");
            return;
        }

        String content = JOptionPane.showInputDialog("Enter message (max 250 characters):");
        if (Message.isValidMessageLength(content)){
        } else {
            JOptionPane.showMessageDialog(null, "Message exceeds 250 characters. Disregarded.");
            disregardedMessages.add(new Message(recipient, content, ++messageCount));
            return;
        }

        String action = JOptionPane.showInputDialog("Choose one: Send/Store/Disregard");
        

        Message msg = new Message(recipient, content, ++messageCount);

        switch (action.toLowerCase()) {
            case "send":
               sentMessages.add(msg);
               totalSent++;
               JOptionPane.showMessageDialog(null,"""
                                                  Message Sent!
                                                  
                                                  ID""" + msg.getMessageId() + "\nHash:" + msg.getMessageHash() + "\nRecipient:"
               + msg.getRecipient() + "\nMessage:" + msg.getContent());
               break;
            case "disregard":
                disregardedMessages.add(msg);
                JOptionPane.showMessageDialog(null,"Message disregarded.");
                break;
            case "store":
                storedMessages.add(msg);
                storeToJSON(msg);
                JOptionPane.showMessageDialog(null, "Message stored to file.");
                break;
            default:
                JOptionPane.showMessageDialog(null, "Invalid action. No action taken");
        }
    }
        
    }
    
        
    private void showAllSentMessages() {
        if (sentMessages.isEmpty()) {
            JOptionPane.showMessageDialog(null, "No messages sent yet.");
            return;
        }

        StringBuilder sb = new StringBuilder("Sent Messages:\n");
        for (Message m : sentMessages) {
            sb.append("ID: ").append(m.getMessageId())
              .append(" | Hash: ").append(m.getMessageHash())
              .append(" | To: ").append(m.getRecipient())
              .append(" | Msg: ").append(m.getContent()).append("\n");
        }
        JOptionPane.showMessageDialog(null, sb.toString());
    }

    private void displayLongestMessage() {
        if (sentMessages.isEmpty()) {
            JOptionPane.showMessageDialog(null, "No messages to evaluate.");
            return;
        }

        Message longest = Collections.max(sentMessages, Comparator.comparingInt(m -> m.getContent().length()));
        JOptionPane.showMessageDialog(null, "Longest Message:\n" + longest.getContent());
    }

    private void searchByID() {
        String id = JOptionPane.showInputDialog("Enter Message ID:");
        for (Message m : sentMessages) {
            if (m.getMessageId().equals(id)) {
                JOptionPane.showMessageDialog(null, "Recipient: " + m.getRecipient()
                        + "\nMessage: " + m.getContent());
                return;
            }
        }
        JOptionPane.showMessageDialog(null, "Message ID not found.");
    }

    private void searchByRecipient() {
        String rec = JOptionPane.showInputDialog("Enter recipient number:");
        StringBuilder found = new StringBuilder();
        for (Message m : sentMessages) {
            if (m.getRecipient().equals(rec)) {
                found.append("ID: ").append(m.getMessageId())
                     .append(" | Msg: ").append(m.getContent()).append("\n");
            }
        }

        if (found.isEmpty()) {
            JOptionPane.showMessageDialog(null, "No messages found for that recipient.");
        } else {
            JOptionPane.showMessageDialog(null, found.toString());
        }
    }

    private void deleteByHash() {
        String hash = JOptionPane.showInputDialog("Enter message hash:");
        for (Iterator<Message> it = sentMessages.iterator(); it.hasNext(); ) {
            Message m = it.next();
            if (m.getMessageHash().equals(hash)) {
                it.remove();
                JOptionPane.showMessageDialog(null, "Message deleted.");
                return;
            }
        }
        JOptionPane.showMessageDialog(null, "Hash not found.");
    }

    private void displayReport() {
        StringBuilder report = new StringBuilder("=== Message Report ===\n");
        for (Message m : sentMessages) {
            report.append("ID: ").append(m.getMessageId())
                  .append(" | Hash: ").append(m.getMessageHash())
                  .append(" | To: ").append(m.getRecipient())
                  .append(" | Msg: ").append(m.getContent()).append("\n");
        }
        report.append("Total Messages Sent: ").append(sentMessages.size());
        JOptionPane.showMessageDialog(null, report.toString());
    }
    @SuppressWarnings("unchecked")
    private void storeToJSON(Message msg){
        JSONObject json = new JSONObject();
        json.put("messageId", msg.getMessageId());
        json.put("recipient", msg.getRecipient());
        json.put("message", msg.getContent());
        json.put("hash", msg.getMessageHash());
       
      try (FileWriter fw = new FileWriter("store_messages.json",true)){
          fw.write(json.toJSONString() + "\n");
      }catch(Exception e){
          e.printStackTrace();
      }
    }
}
