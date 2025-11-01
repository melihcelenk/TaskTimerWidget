import tkinter as tk
from tkinter import ttk
from datetime import timedelta

ACTIVE_BG = "#FFEB00"   # sarı
INACTIVE_BG = "#222222" # koyu gri
TEXT_COLOR = "#000000"
INACTIVE_TEXT_COLOR = "#FFFFFF"

class TaskFrame(ttk.Frame):
    def __init__(self, master, app, initial_edit_mode=False):
        super().__init__(master)
        self.app = app
        self.task_name = ""
        self.elapsed_seconds = 0
        self.is_active = False

        self["padding"] = (10, 8)
        self.columnconfigure(0, weight=1)

        # Stil: koyu arka planlı kutucuk
        self.container = tk.Frame(self, bg=INACTIVE_BG)
        self.container.grid(row=0, column=0, sticky="nsew")
        self.container.columnconfigure(0, weight=1)

        # Başlık ve süre etiketleri
        self.name_label = tk.Label(self.container, text="yeni task", bg=INACTIVE_BG,
                                   fg=INACTIVE_TEXT_COLOR, font=("Segoe UI", 12, "bold"))
        self.name_label.grid(row=0, column=0, sticky="w", padx=14, pady=(10, 0))

        self.time_label = tk.Label(self.container, text="0m 0s", bg=INACTIVE_BG,
                                   fg=INACTIVE_TEXT_COLOR, font=("Segoe UI", 12))
        self.time_label.grid(row=1, column=0, sticky="w", padx=14, pady=(6, 12))

        # İlk oluşturulurken kutunun içinde edit alanı gösterilsin istendi
        self.name_entry = None
        if initial_edit_mode:
            self._enter_edit_mode()
        else:
            self.task_name = "yeni task"

        # Tıklama ile başlat/duraklat
        self.container.bind("<Button-1>", self.on_click)
        self.name_label.bind("<Button-1>", self.on_click)
        self.time_label.bind("<Button-1>", self.on_click)

        # Görsel olarak hafif yuvarlak köşe efekti için padding
        self["borderwidth"] = 0

    def _enter_edit_mode(self):
        if self.name_entry is not None:
            return
        # Mevcut label’ı gizle, yerine Entry koy
        self.name_label.grid_remove()
        self.name_entry = tk.Entry(self.container, font=("Segoe UI", 12, "bold"))
        self.name_entry.grid(row=0, column=0, sticky="ew", padx=12, pady=(10, 0))
        self.name_entry.insert(0, self.task_name if self.task_name else "")
        self.name_entry.focus()
        self.name_entry.bind("<Return>", self._commit_name)
        self.name_entry.bind("<Escape>", self._cancel_edit)

    def _commit_name(self, _event=None):
        text = self.name_entry.get().strip()
        if not text:
            text = "isimsiz task"
        self.task_name = text
        self.name_label.config(text=self.task_name)
        self.name_entry.destroy()
        self.name_entry = None
        self.name_label.grid()
        # Enter’a basınca task oluşmuş kabul edilir; otomatik başlatmıyoruz.

    def _cancel_edit(self, _event=None):
        self.name_entry.destroy()
        self.name_entry = None
        self.name_label.grid()

    def set_active(self, active: bool):
        self.is_active = active
        bg = ACTIVE_BG if active else INACTIVE_BG
        fg = TEXT_COLOR if active else INACTIVE_TEXT_COLOR
        for widget in (self.container, self.name_label, self.time_label):
            widget.configure(bg=bg)
        self.name_label.configure(fg=fg)
        self.time_label.configure(fg=fg)

    def on_click(self, _event=None):
        # Aktif ise duraklat; değilse bunu aktif yap
        if self.is_active:
            self.app.pause_current_task()
        else:
            self.app.activate_task(self)

    def increment_one_second(self):
        if not self.is_active:
            return
        self.elapsed_seconds += 1
        self._refresh_time_text()

    def _refresh_time_text(self):
        # Görsel: "12m 6s" formatı
        minutes = self.elapsed_seconds // 60
        seconds = self.elapsed_seconds % 60
        self.time_label.config(text=f"{minutes}m {seconds}s")


class TaskTimerApp(tk.Tk):
    def __init__(self):
        super().__init__()
        self.title("Task Widget Timer")
        self.configure(bg="#111111")
        self.geometry("260x320")  # küçük, widget gibi
        self.resizable(False, False)

        # Ana kap: task listesi + ekleme düğmesi
        self.task_list_frame = ttk.Frame(self, padding=(6, 6, 6, 0))
        self.task_list_frame.pack(fill="both", expand=True)

        self.scroll_canvas = tk.Canvas(self.task_list_frame, bg="#111111", highlightthickness=0)
        self.scroll_canvas.pack(side="left", fill="both", expand=True)

        self.scrollbar = ttk.Scrollbar(self.task_list_frame, orient="vertical",
                                       command=self.scroll_canvas.yview)
        self.scrollbar.pack(side="right", fill="y")
        self.scroll_canvas.configure(yscrollcommand=self.scrollbar.set)

        self.inner_frame = ttk.Frame(self.scroll_canvas)
        self.inner_frame.bind(
            "<Configure>",
            lambda e: self.scroll_canvas.configure(scrollregion=self.scroll_canvas.bbox("all"))
        )
        self.scroll_canvas.create_window((0, 0), window=self.inner_frame, anchor="nw", width=240)

        # + butonu
        self.add_button = ttk.Button(self, text="+", command=self.add_new_task, width=3)
        self.add_button.pack(pady=6)

        # Durum
        self.tasks = []
        self.active_task = None

        # Zamanlayıcı döngüsü
        self.after(1000, self._tick)

        # Modern ttk tema renkleri
        style = ttk.Style(self)
        try:
            style.theme_use("clam")
        except tk.TclError:
            pass

    def add_new_task(self):
        task = TaskFrame(self.inner_frame, app=self, initial_edit_mode=True)
        task.pack(fill="x", padx=4, pady=6)
        self.tasks.append(task)

    def activate_task(self, task: TaskFrame):
        if self.active_task is task:
            return
        if self.active_task is not None:
            self.active_task.set_active(False)
        self.active_task = task
        self.active_task.set_active(True)

    def pause_current_task(self):
        if self.active_task is None:
            return
        self.active_task.set_active(False)
        self.active_task = None

    def _tick(self):
        # Her saniye aktif task varsa süreyi ilerlet
        if self.active_task is not None:
            self.active_task.increment_one_second()
        self.after(1000, self._tick)


if __name__ == "__main__":
    app = TaskTimerApp()
    app.mainloop()
