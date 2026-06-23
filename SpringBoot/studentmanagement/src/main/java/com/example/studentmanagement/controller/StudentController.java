package com.example.studentmanagement.controller;

import com.example.studentmanagement.model.Student;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;

@Controller
public class StudentController {

    @GetMapping("/student/info")
    public String info(Model model) {

        Student student = new Student(
                "Nguyen Van A",
                20,
                "Cong nghe thong tin");

        model.addAttribute("student", student);

        return "student/info";
    }
}