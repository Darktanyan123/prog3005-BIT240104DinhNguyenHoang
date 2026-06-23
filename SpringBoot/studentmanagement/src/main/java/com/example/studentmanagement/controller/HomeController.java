package com.example.studentmanagement.controller;

import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.ResponseBody;

@Controller
@RequestMapping("/home")
public class HomeController {

    @GetMapping("/index")
    @ResponseBody
    public String index() {
        return "Welcome to Spring Boot";
    }

    @GetMapping("/about")
    @ResponseBody
    public String about() {
        return "Nguyen Hoang";
    }

    @GetMapping("/contact")
    @ResponseBody
    public String contact() {
        return "nguyenhoang@gmail.com";
    }
}