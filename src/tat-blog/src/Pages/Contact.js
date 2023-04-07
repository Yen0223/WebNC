import React, { useEffect } from "react";
const Contact = () => {
  useEffect(() => {
    document.title = "Trang liên hệ";
  }, []);
  return (
  <h1>
    Đây là trang liên hệ
    </h1>
  );
}

export default Contact;