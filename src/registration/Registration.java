/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Main.java to edit this template
 */
package registration;

import javax.swing.JOptionPane;


/**
 *
 * @author RC_Student_lab
 */
public class Registration {
    private static String userName;
    private static String password;
    private static String cellPhoneNumber;
    private static String firstName;
    private static String lastName;
    
     public static void setFirstName(String firstname) {
        firstName = firstname;
    }

    public static String getFirstName() {
        return firstName;
    }

    public static void setUserName(String username) {
        userName = username;
    }

    public static String getUserName() {
        return userName;
    }

    public static void setPassword(String passw0rd) {
        password = passw0rd;
    }

    public static String getPassword() {
        return password;
        
        }

    public static void setLastname(String lastname) {
        lastName = lastname;
    }

    public static String getLastname() {
        return lastName;
        
    }
    
    public static void setCellNumber(String cellphonenumber){
        cellPhoneNumber = cellphonenumber;
    }
    
    public static String getCellNumber(){
        return cellPhoneNumber;
    }

    /**
     * @param args the command line arguments
     */
    public static void main(String[] args) {
        
         
         Login login = new Login(firstName, lastName);
         
         JOptionPane.showMessageDialog(null,"Registration");
        
        firstName =JOptionPane.showInputDialog(" Please enter your first name");
        lastName =JOptionPane.showInputDialog("Please enter your lastname");
        userName =JOptionPane.showInputDialog("Enter your user name.(The username must have 5 or less characters and contain an underscore)");
        password =JOptionPane.showInputDialog("Create a password.(Password must contain at least 8 characters, a capital letter, a number and l speacial character.)");
        cellPhoneNumber =JOptionPane.showInputDialog("Please enter cellphone number with international country code");
        
        String regStatus = login.registerUser(userName, password, cellPhoneNumber, firstName, lastName);
        JOptionPane.showMessageDialog(null, regStatus);
        
           if (regStatus.equals("Registration successful.")){
               JOptionPane.showMessageDialog(null,"Login" );
               String loginUser = JOptionPane.showInputDialog("Enter your username");
               String loginPass = JOptionPane.showInputDialog("Enter password");
               
                String loginStatus = login.returnLoginStatus(loginUser, loginPass);
                JOptionPane.showMessageDialog(null, loginStatus);
                
                if(loginStatus.startsWith("Welcome")){
                    ChatApp chatApp = new ChatApp();
                    chatApp.start();
                }
           }

        
        
    }
    
}
